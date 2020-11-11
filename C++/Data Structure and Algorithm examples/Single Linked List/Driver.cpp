#include<iostream>
#include"SingleLinkedList.h"

using std::cout;
using std::cin;
using std::endl;

void listSearch(SingleLinkedList &list, int target)
{
	cout << "Search " << target << " = " << list.search(target).first << endl;
	list.showSOLL();
	cout << endl << endl;
}

void listSearchMTF(SingleLinkedList &list, int target)
{
	cout << "Search_mtf " << target << " = " << list.search_mtf(target) << endl;
	list.showSOLL();
	cout << endl << endl;
}

void listSearchT(SingleLinkedList &list, int target)
{
	cout << "Search_t " << target << " = " << list.search_t(target) << endl;
	list.showSOLL();
	cout << endl << endl;
}

void listInsert(SingleLinkedList &list, int target)
{
	cout << "\nInserting " << target << "...\n";
	list.insert(target);
	listSearch(list, 1);
}

void listInsertMTF(SingleLinkedList &list, int target)
{
	cout << "\nInserting " << target << "...\n";
	list.insert(target);
	list.showSOLL();
	listSearchMTF(list, 1);
}

void listInsert_T(SingleLinkedList &list, int target)
{
	cout << "\nInserting " << target << "...\n";
	list.insert(target);
	list.showSOLL();
	listSearchT(list, 1);
}



int main()
{
	cout << "Creating list1..." << endl;
	SingleLinkedList list3;
	listSearch(list3, 1);

	listInsert(list3, 1);
	listInsert(list3, 2);
	listInsert(list3, 3);
	listInsert(list3, 3);
	listInsert(list3, 4);
	listInsert(list3, 5);
	listSearch(list3, 6);

	cout << "\nCreating list2..." << endl;
	SingleLinkedList list;
	cout << "Search_mtf 1 = " << list.search_mtf(1) << endl;

	listInsertMTF(list, 1);
	cout << "\nSearch_mtf 4 = " << list.search_mtf(4) << endl;

	listInsertMTF(list, 2);
	listInsertMTF(list, 3);
	listInsertMTF(list, 4);
	listInsertMTF(list, 5);
	listSearchMTF(list, 6);
	listSearchMTF(list, 3);
	cout << "\nCreating list3..." << endl;
	SingleLinkedList list2;
	cout << "Search_t 1 = "; cout << list2.search_t(1) << endl;

	listInsert_T(list2, 1);
	listInsert_T(list2, 2);
	listInsert_T(list2, 3);
	listSearchT(list2, 3);
	listInsert_T(list2, 4);

	listInsert_T(list2, 5);
	listSearchT(list2, 5);
	listSearchT(list2, 5);
	listSearchT(list2, 5);
	listSearchT(list2, 5);

	listSearchT(list2, 6);

	cout << endl;
	system("pause");
}