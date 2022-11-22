

# Password-tool

Password-tools is a Project that provides a password generator and a password manager

#Manager
The manager uses the AES encryption to encrypt the password and then store it in an .dat file. The first time ever you start the application, you have to input the location where the password.dat should be stored. Later you could change it when you type in the manager console setDir. Every time you restart the application, you have to input your key. If you use it for the first time, you could generate one with --gen. To get a list of all commands, just type "?" in the command line

#Generator
The generator can generate you a password between 10 and 2.147.483.647 *(the program actually tells you 536870911 is the max because that's the longest password that could be generated at once and for real: who wants a password that is more than 500mb big?)* characters long. The Password will be shown in the Console (if it has less than 250,000 characters) and will be written to a file (unless you turn that off by typing "wf" in the command line). The first time you start the application, you have to input the location where the generated passwords are stored. Later you could change it by typing in setDir. To get a list of all commands, just type "?" in the command line.

*Ther actually are two hidden commands: d and f. d stands for diagnostics just type in d and you are able to see a description of d and f in the help(?) menue.*
 
 #resources
 1. The Icon is made by Ivan Abirawa. You could download it at [flaticon.com](https://www.flaticon.com/de/kostenloses-icon/sperren_3183023?related_id=3183023&origin=search#): 
 
