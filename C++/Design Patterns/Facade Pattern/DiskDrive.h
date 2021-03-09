//
// Created by gregathee on 12/22/2020.
//

#ifndef FACADE_PATTERN_DISKDRIVE_H
#define FACADE_PATTERN_DISKDRIVE_H

#endif //FACADE_PATTERN_DISKDRIVE_H

class DiskDrive
{
private:
    void SearchForData();
    void LoadData();
    void SendToRam();
    void AllocateMemory();
    void Write();
public:
    void ReadFromDisk();
    void WriteToDisk();
};

