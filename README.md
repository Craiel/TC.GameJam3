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
