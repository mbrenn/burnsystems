
BURNSYSTEMS_CS_FILES = $(shell find src/ -type f -name *.cs)
BURNSYSTEMS_TESTS_CS_FILES = $(shell find tests/ -type f -name *.cs)

all: bin/BurnSystems.dll bin/BurnSystems.UnitTests.dll

bin/BurnSystems.dll: $(BURNSYSTEMS_CS_FILES)
	xbuild src/BurnSystems.csproj
	mkdir -p bin
	cp src/bin/Debug/BurnSystems.dll bin/BurnSystems.dll

bin/BurnSystems.UnitTests.dll: $(BURNSYSTEMS_TESTS_CS_FILES)
	xbuild tests/BurnSystems.UnitTests/BurnSystems.UnitTests.csproj
	mkdir -p bin
	cp tests/BurnSystems.UnitTests/nunit.framework.dll bin/nunit.framework.dll
	cp tests/BurnSystems.UnitTests/bin/Debug/BurnSystems.UnitTests.dll bin/BurnSystems.UnitTests.dll

.PHONY: install
install: all
	mkdir -p ~/lib/mono
	cp bin/* ~/lib/mono

.PHONY: test
test: all
	nunit-console -labels bin/BurnSystems.UnitTests.dll

.PHONY: clean
clean:
	rm -fR src/bin
	rm -fR src/obj
	rm -fR bin
