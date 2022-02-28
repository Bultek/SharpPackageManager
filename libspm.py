import os
import shutil
import argparse
libspm = False
version=0.2
def main():
    if os.path.exists("C:\\SPM\\modules"):
        libspm = True
def RmFile(filename):
    os.remove(filename)
def CpFile(filename, dest):
    os.copy(filename, dest)
def MkFile(filename):
    os.touch(filename)
def MkDir(directory):
    os.mkdir(directory)
main();