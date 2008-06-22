MCS_FLAGS = -warn:4 -langversion:LINQ
MCS = gmcs

CSHARP_FILES = *.cs \
	Collections/*.cs \
	Graphics/*.cs \
	Interfaces/*.cs IO/*.cs \
	Logging/*.cs \
	Net/*.cs \
	Properties/*.cs \
	Synchronisation/*.cs \
	Test/*.cs \
	UnitTests/*.cs

BURNSYSTEMS_REFERENCES = System.Drawing.dll,System.Web.dll

all: 	bin/Release/BurnSystems.dll

bin/Release/BurnSystems.dll: $(CSHARP_FILES)
	mkdir -p bin
	mkdir -p bin/Release
	$(MCS) $(MCS_FLAGS) -out:bin/Release/BurnSystems.dll -target:library -r:$(BURNSYSTEMS_REFERENCES) $(CSHARP_FILES) 

.PHONY: clean
clean:
	rm -rf bin
