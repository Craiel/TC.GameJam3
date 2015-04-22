# Build instructions
	You'll need SDL to build the project. Either install SDL as package or build the one in ext/ directory by cd-ing into directory and typing:

	./configure
	make

	You can skip 'make install' since project is configured to find headers and libraries in there.

	After SDL is built, cd to the project root and type:

	cmake .

	This'll generate standard GNU makefiles. If you're using an IDE, type 'cmake' to see list of possible project files that cmake can generate for you (Netbeans, Eclipse, CodeBlocks, KDevelop...)and then you can generate that instead. Example:

	cmake . -G"Eclipse CDT4 - Unix Makefiles"

	After cmake is run, Makefiles and project files will be generated. Run:

	make

	and wait until 'tuesday_devs' executable is built.


## Command notes
How to serve repository:

	git daemon --reuseaddr --base-path=. --export-all --verbose

To clone/pull from served repo:

	git clone git://<ipaddress>/ <target>
