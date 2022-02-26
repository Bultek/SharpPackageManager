import os
import shutil
import argparse
libspm = False
version=0.1
def main():
    if os.path.exists("C:\\SPM\\modules"):
        libspm = True
main();