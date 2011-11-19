
all: BurnSystems.dll BurnSystems.UnitTests.dll


BurnSystems.dll:
	xbuild src/BurnSystems.csproj
	mkdir -p bin
	cp src/bin/Debug/BurnSystems.dll bin/BurnSystems.dll

BurnSystems.UnitTests.dll:
	xbuild tests/BurnSystems.UnitTests/BurnSystems.UnitTests.csproj
	mkdir -p bin
	cp tests/BurnSystems.UnitTests/bin/Debug/BurnSystems.UnitTests.dll bin/BurnSystems.UnitTests.dll

.PHONY: install
install: all
	mkdir -p ~/lib/mono
	cp bin/* ~/lib/mono

.PHONY: clean
clean:
	rm -fR src/bin
	rm -fR src/obj
	rm -fR bin
