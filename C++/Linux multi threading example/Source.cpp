#include<iostream>
#include<string>
#include<cstring>
#include<fstream>
#include<sstream>
#include<deque>
#include <stdlib.h>
#include <unistd.h> 
#include <stdio.h>
#include<time.h>
#include<signal.h>
#include<sys/types.h>
#include <fcntl.h>
#include <sys/stat.h>
#include <unistd.h>
#include <sys/wait.h>
#define ALARM 1000000

namespace patch
{
	template < typename T > std::string to_string ( const T& n )
	{
		std::ostringstream stm;
		stm << n;
		return stm.str ();
	}
}

using namespace std;

void wake_up ( int signal_number );
//********************************************job class***************************
class Job
{
private:
	int jobLength;
	int priority;
	int ID;

public:
	Job ( int IDIn, int priorityIn, int jobLengthIn )
	{
		jobLength = jobLengthIn;
		priority = priorityIn;
		ID = IDIn;
	}

	Job ( const Job& oldJob )
	{
		jobLength = oldJob.jobLength;
		priority = oldJob.priority;
		ID = oldJob.ID;
	}

	Job () {}

	void performTask ()
	{
		cout << "Job number " << ID << " is executing under priority " << priority
			<< " and will take " << jobLength << " to complete " << endl;

		for ( int i = 0; i < jobLength; i++ )
		{
			cout << "Job number " << ID << " will finish in " << jobLength
				<< " more quantums..." << endl;
			switch ( priority )
			{
			case 1: break;
			case 2: i = 0; break;
			case 3:
				cout << "Job number " << ID << " is waiting for (ctrl+c)" << endl;
				signal ( SIGINT, wake_up );
				sleep ( 5 );
				/*These jobs will be run and then blocked for the input.
				It will become ready to run again when user types the interrupt key ^C,
				then it will be run to the finish.*/
				break;
			case 4: if ( i == 0 ) { sleep ( 5 ); } break;
			}
			jobLength--;
		}
	}

	int getJobLength () { return jobLength; }
	int getPriority () { return priority; }
	int getID () { return ID; }

	void setJobLength ( int jobLengthIn ) { jobLength = jobLengthIn; }
	void setPriority ( int priorityIn ) { priority = priorityIn; }
	void setID ( int IDIn ) { ID = IDIn; }
};
//***********************************************global variables********************
int quantumTime = 3;
string taskQueueCountFileName = "TaskQueueCount.txt";;
string taskQueueFileName = "TaskQueue.txt";
string serverQueueCountFileName = "ServerQueueCount.txt";
string serverQueueFileName = "ServerQueue.txt";
string powerUserQueueCountFileName = "PowerUserQueueCount.txt";
string powerUserQueueFileName = "PowerUserQueue.txt";
string userQueueCountFileName = "UserQueueCount.txt";
string userQueueFileName = "UserQueue.txt";

deque<Job>* jobDeque = new deque<Job> ();
int* jobID = new int ( 0 );
int MAX_JOB_QUEUE = 20;
//***********************************************prototypes**********************
void unpack ( const string& fileName, const int& queueSize );
void pack ( const string& fileName, const int& queueSize );
void enQueue ( int*& queueCount, const Job& jobIn, const string& fileName );
Job deQueue ( int*& queueCount, const string& fileName );
void clearFile ( string fileName );
void jobGenerator ();
void jobScheduler ();
void down ( int* semid, char* semname, string str );
void up ( int semid, char* semname );
void incrementQueueCount ( string fileName );
void decrementQueueCount ( string fileName );
int getQueueCount ( string fileName );
void initQueueCountFiles ();
string queueNameToQueueCount ( string fileName );

//**************************************************main******************************
int main ()
{
	int pid = 0;
	int pid2 = 0;
	int status = 0;

	clearFile ( taskQueueFileName );
	clearFile ( serverQueueFileName );
	clearFile ( powerUserQueueFileName );
	clearFile ( userQueueFileName );
	clearFile ( taskQueueCountFileName );
	clearFile ( serverQueueCountFileName );
	clearFile ( powerUserQueueCountFileName );
	clearFile ( userQueueCountFileName );
	initQueueCountFiles ();

	if ( pid = fork () > 0 )
	{
		if ( pid2 = fork () > 0 )
		{
			for ( int i = 0; i < 10; i++ )
			{
				cout << "taskQueueCount: " << getQueueCount ( taskQueueCountFileName ) << endl;
				cout << "serverQueueCount: " << getQueueCount ( serverQueueCountFileName ) << endl;
				cout << "powerUserQueueCount: " << getQueueCount ( powerUserQueueCountFileName ) << endl;
				cout << "userQueueCount: " << getQueueCount ( userQueueCountFileName ) << endl;
				sleep ( 1 );
			}
			exit ( 0 );
		}
		else
		{
			cout << "Starting job generator..." << endl;
			jobGenerator ();
			sleep ( 10 );
			exit ( 0 );
		}
		exit ( 0 );
	}
	else
	{
		cout << "Starting job scheduler..." << endl;
		jobScheduler ();
		cout << "Terminating scheduler..." << endl;
		exit ( 0 );
	}
	return ( 1 );
}
//*********************************************functions*******************************


void jobScheduler ()
{
	int pid = 0;
	int semid;
	char semname[ 10 ];
	strcpy ( semname, "mutex.txt" );
	for ( int i = 5; i > 0; i-- )
	{
		cout << "scheduler loop " << i << endl;
		if ( getQueueCount ( taskQueueCountFileName ) != 0 )
		{
			cout << "attempting to fork schedual task queue..." << endl;
			if ( pid = fork () > 0 )
			{
				cout << "Schedualing 1st priority job..." << endl;
				down ( &semid, semname, "scheduler taskQueue" );
				Job tempJob ( deQueue ( getQueueCount ( userQueueCountFileName ), taskQueueFileName ) );
				up ( semid, semname );
				tempJob.performTask ();
				sleep ( 1 );
				exit ( 0 );
			}
		}
		else if ( getQueueCount ( serverQueueCountFileName ) != 0 )
		{
			cout << "attempting to fork schedual server queue..." << endl;
			if ( pid = fork () > 0 )
			{
				cout << "Schedualing 2nd priority job..." << endl;
				down ( &semid, semname, "scheduler serverQueue" );
				Job tempJob ( deQueue ( getQueueCount ( serverQueueCountFileName ), serverQueueFileName ) );
				up ( semid, semname );
				tempJob.performTask ();
				if ( tempJob.getJobLength () != 0 )
				{
					down ( &semid, semname, "scheduler serverQueue" );
					enQueue ( getQueueCount ( serverQueueCountFileName ), tempJob, serverQueueFileName );
					up ( semid, semname );
				}
				sleep ( 1 );
				exit ( 0 );
			}
		}
		else if ( getQueueCount ( powerUserQueueCountFileName ) != 0 )
		{
			cout << "attempting to fork schedual powerUser queue..." << endl;
			if ( pid = fork () > 0 )
			{
				cout << "Schedualing 3rd priority job..." << endl;
				down ( &semid, semname, "scheduler powerUserQueue" );
				Job tempJob ( deQueue ( getQueueCount ( powerUserQueueCountFileName ), powerUserQueueFileName ) );
				up ( semid, semname );
				tempJob.performTask ();
				sleep ( 1 );
				exit ( 0 );
			}
		}
		else if ( getQueueCount ( userQueueCountFileName ) != 0 )
		{
			cout << "attempting to fork schedual user queue..." << endl;
			if ( pid = fork () > 0 )
			{
				cout << "Schedualing 4th priority job..." << endl;
				down ( &semid, semname, "scheduler userQueue" );
				Job tempJob ( deQueue ( getQueueCount ( userQueueCountFileName ), userQueueFileName ) );
				up ( semid, semname );
				tempJob.performTask ();
				sleep ( 1 );
				exit ( 0 );
			}
		}
		sleep ( 1 );
	}
}

void jobGenerator ()
{
	int pid = 0;
	int semid;
	char semname[ 10 ];
	strcpy ( semname, "mutex.txt" );
	srand ( time ( 0 ) );

	for ( int i = 5; i > 0; i-- )
	{
		cout << "generator loop " << i << endl;
		int priority = rand () % 100 + 1;
		int jobLength = rand () % 3 + 1;
		*jobID = *jobID + 1;
		if ( priority >= 1 && priority <= 10 )
		{
			if ( *taskQueueCount != MAX_JOB_QUEUE )
			{
				if ( pid = fork () > 0 )
				{
					cout << "Generating 1st priority job..." << endl;
					Job newJob ( *jobID, 1, jobLength );
					down ( &semid, semname, "Generator taskQueue" );
					cout << "taskQueueCount before enQueue: " << getQueueCount ( taskQueueCountFileName ) << endl;
					enQueue ( getQueueCount ( taskQueueCountFileName ), newJob, taskQueueFileName );
					cout << "taskQueueCount after enQueue: " << getQueueCount ( taskQueueCountFileName ) << endl;
					up ( semid, semname );
					sleep ( 1 );
					exit ( 0 );
				}
			}
		}
		else if ( priority >= 11 && priority <= 30 )
		{
			if ( *serverQueueCount != MAX_JOB_QUEUE )
			{
				if ( pid = fork () > 0 )
				{
					cout << "Generating 2nd priority job..." << endl;
					Job newJob ( *jobID, 2, jobLength );
					down ( &semid, semname, "Generator serverQueue" );
					cout << "serverQueueCount before enQueue: " << getQueueCount ( serverQueueCountFileName ) << endl;
					enQueue ( getQueueCount ( serverQueueCountFileName ), newJob, serverQueueFileName );
					cout << "serverQueueCount after enQueue: " << getQueueCount ( serverQueueCountFileName ) << endl;
					up ( semid, semname );
					sleep ( 1 );
					exit ( 0 );
				}
			}
		}
		else if ( priority >= 31 && priority <= 50 )
		{
			if ( *powerUserQueueCount != MAX_JOB_QUEUE )
			{
				if ( pid = fork () > 0 )
				{
					cout << "Generating 3rd priority job..." << endl;
					Job newJob ( *jobID, 3, jobLength );
					down ( &semid, semname, "Generator powerQueue" );
					cout << "powerUserQueueCount before enQueue: " << getQueueCount ( powerUserQueueCountFileName ) << endl;
					enQueue ( getQueueCount ( powerUserQueueCountFileName ), newJob, powerUserQueueFileName );
					cout << "powerUserQueueCount after enQueue: " << getQueueCount ( powerUserQueueCountFileName ) << endl;
					up ( semid, semname );
					sleep ( 1 );
					exit ( 0 );
				}
			}
		}
		else
		{
			if ( *userQueueCount != MAX_JOB_QUEUE )
			{
				if ( pid = fork () > 0 )
				{
					cout << "Generating 4th priority job..." << endl;
					Job newJob ( *jobID, 4, jobLength );
					down ( &semid, semname, "Generator userQueue" );
					cout << "userQueueCount before enQueue: " << getQueueCount ( userQueueCountFileName ) << endl;
					enQueue ( getQueueCount ( userQueueCountFileName ), newJob, userQueueFileName );
					cout << "userQueueCount after enQueue: " << getQueueCount ( userQueueCountFileName ) << endl;
					up ( semid, semname );
					sleep ( 1 );
					exit ( 0 );
				}
			}
		}
		sleep ( 1 );
	}
}

void down ( int* semid, char* semname, string str )
{
	while ( *semid = creat ( semname, 0 ) == -1 ) /* && error == EACCES)*/
	{
		cout << str << "is down " << getpid () << endl;
		sleep ( 3 );
	}
}

void up ( int semid, char* semname )
{
	close ( semid );
	unlink ( semname );
	printf ( "up %s: I am waked up.\n", semname );
}

void wake_up ( int signal_number )
{
	cout << "I will wake up in 1 second.\n";
	ualarm ( ALARM, 0 );
}

void enQueue ( int queueCount, const Job& jobIn, const string& fileName )
{
	unpack ( fileName, queueCount );
	jobDeque->push_back ( jobIn );
	incrementQueueCount ( queueNameToQueueCount ( fileName ) );
	pack ( fileName, queueCount );
}

Job deQueue ( int queueCount, const string& fileName )
{
	unpack ( fileName, queueCount );
	Job jobOut ( jobDeque->front () );
	jobDeque->pop_front ();
	decrementQueueCount ( queueNameToQueueCount ( fileName ) );
	pack ( fileName, queueCount );
	return jobOut;
}

string queueNameToQueueCount ( string fileName )
{
	switch ( fileName )
	{
	case taskQueueFileName: return taskQueueCountFileName;
	case serverQueueFileName: return serverQueueCountFileName;
	case powerUserQueueFileName: return powerUserQueueCountFileName;
	case userQueueFileName: return userQueueCountFileName;
	default: return "";
	}
}

void incrementQueueCount ( string fileName )
{
	ifstream inQueueCount ( fileName, ios::in );
	if ( !inQueueCount )
	{
		cerr << fileName << " could not be opened" << endl;
		exit ( EXIT_FAILURE );
	}
	string stringCount;
	inQueueCount >> stringCount;
	inQueueCount.close ();
	stringstream sp ( stringCount );
	int intCount;
	sp >> intCount;
	intCount++;

	ofstream outQueueCount ( fileName, ios::in );
	if ( !outQueueCount )
	{
		cerr << fileName << " could not be opened" << endl;
		exit ( EXIT_FAILURE );
	}
	outQueueCount << patch::to_string ( intCount );
	outQueueCount.close ();
}

void decrementQueueCount ( string fileName )
{
	ifstream inQueueCount ( fileName, ios::in );
	if ( !inQueueCount )
	{
		cerr << fileName << " could not be opened" << endl;
		exit ( EXIT_FAILURE );
	}
	string stringCount;
	inQueueCount >> stringCount;
	inQueueCount.close ();
	stringstream sp ( stringCount );
	int intCount;
	sp >> intCount;
	intCount--;

	ofstream outQueueCount ( fileName, ios::in );
	if ( !outQueueCount )
	{
		cerr << fileName << " could not be opened" << endl;
		exit ( EXIT_FAILURE );
	}
	outQueueCount << patch::to_string ( intCount );
	outQueueCount.close ();
}

int getQueueCount ( string fileName )
{
	ifstream inQueueCount ( fileName, ios::in );
	if ( !inQueueCount )
	{
		cerr << fileName << " could not be opened" << endl;
		exit ( EXIT_FAILURE );
	}
	string stringCount;
	inQueueCount >> stringCount;
	inQueueCount.close ();
	stringstream sp ( stringCount );
	int intCount;
	sp >> intCount;
	return intCount;
}

void clearFile ( string fileName )
{
	ofstream jobQueue ( fileName.c_str (), ios::in | ofstream::trunc );
	if ( !jobQueue )
	{
		cerr << fileName << " could not be opened" << endl;
		exit ( EXIT_FAILURE );
	}
	jobQueue.close ();
}

void initQueueCountFiles ()
{
	ofstream taskCount ( taskQueueCountFileName, ios::in );
	if ( !taskCount )
	{
		cerr << taskQueueCountFileName << " could not be opened" << endl;
		exit ( EXIT_FAILURE );
	}
	ofstream serverCount ( serverQueueCountFileName, ios::in );
	if ( !serverCount )
	{
		cerr << serverQueueCountFileName << " could not be opened" << endl;
		exit ( EXIT_FAILURE );
	}
	ofstream powerCount ( powerUserQueueCountFileName, ios::in );
	if ( !powerCount )
	{
		cerr << powerUserQueueCountFileName << " could not be opened" << endl;
		exit ( EXIT_FAILURE );
	}
	ofstream userCount ( userQueueCountFileName, ios::in );
	if ( !userCount )
	{
		cerr << userQueueCountFileName << " could not be opened" << endl;
		exit ( EXIT_FAILURE );
	}
	taskCount << "0";
	serverCount << "0";
	powerCount << "0";
	userCount << "0";

	taskCount.close ();
	serverCount.close ();
	powerCount.close ();
	userCount.close ();
}

void pack ( const string& fileName, const int& queueSize )
{
	if ( queueSize == 0 )
	{
		clearFile ( fileName );
		return;
	}
	ofstream jobQueue ( fileName.c_str (), ios::in );
	if ( !jobQueue )
	{
		cerr << fileName << " could not be opened" << endl;
		exit ( EXIT_FAILURE );
	}

	for ( int i = queueSize; i > 0; i-- )
	{
		Job jobToLoad ( jobDeque->front () );
		jobQueue << patch::to_string ( jobToLoad.getID () ) << " "
			<< patch::to_string ( jobToLoad.getPriority () ) << " "
			<< patch::to_string ( jobToLoad.getJobLength () ) << "\n";
		jobDeque->pop_front ();
	}
	jobQueue.close ();
}

void unpack ( const string& fileName, const int& queueSize )
{
	if ( queueSize == 0 ) return;
	string jobLengthString;
	int jobLengthInt;
	string jobIDString;
	int jobIDInt;
	string jobPriorityString;
	int jobPriorityInt;

	ifstream jobQueue ( fileName.c_str (), ios::in );
	if ( !jobQueue )
	{
		cerr << fileName << " could not be opened" << endl;
		exit ( EXIT_FAILURE );
	}

	for ( int i = queueSize; i > 0; i-- )
	{
		jobQueue >> jobIDString >> jobPriorityString >> jobLengthString;
		stringstream IDStream ( jobIDString );
		stringstream priorityStream ( jobPriorityString );
		stringstream jobLengthStream ( jobLengthString );
		IDStream >> jobIDInt;
		priorityStream >> jobPriorityInt;
		jobLengthStream >> jobLengthInt;
		Job tempJob ( jobIDInt, jobPriorityInt, jobLengthInt );
		jobDeque->push_back ( tempJob );
	}
	jobQueue.close ();
	clearFile ( fileName );
}