# C# CoreUtils

Unix core utilities implemented in C#.  I was inspired by a project
done in Haskell [by Gandalf](https://github.com/Gandalf-/coreutils). 
I liked the consciseness of the functional language and thought this 
would be a good project to attempt to add more functional language 
concepts to my C# programming.

### cat

Implements only the very basic functionality of the cat utility.

Input is read in chunks to better handle very large files.

.Net console is used to not have to deal with tty's or identify the
host OS to determine where to read STDIN.
