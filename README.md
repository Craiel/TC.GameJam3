## Required Software
- [Unity 5](https://unity3d.com/)
- [Gimp](http://www.gimp.org/)
- [Git](http://git-scm.com/)
- [MyPaint](http://mypaint.intilinux.com/)
- [Blender](http://www.blender.org/)

## Optional Software
- [SourceTree](http://www.sourcetreeapp.com/)

## Command notes
How to serve repository:

	git daemon --reuseaddr --base-path=. --export-all --verbose

To clone/pull from served repo:

	git clone git://<ipaddress>/ <target>


## How not to be annoyed with Git cmdline when it keeps asking for password
Enable the password caching:

	git config --global credential.helper cache

Set the cache timeout to 1 hour:

	git config --global credential.helper 'cache --timeout=3600'
