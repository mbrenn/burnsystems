pipeline {
    agent any

    stages {
	    stage ('Build') 
        {
 			// Shell build step
            sh """ 
            cd src
            dotnet build
            dotnet test BurnSystems.Tests/bin/Debug/net5.0/BurnSystems.Tests.dll --logger "trx;LogFileName=test.trx"
            cd .. 
            """ 
	    }
    }
}