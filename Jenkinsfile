pipeline {
    agent any

    stages {
	    stage ('Build') 
        {
            steps 
            {
 			    // Shell build step
                sh """ 
                    cd src
                    dotnet build

                    cd .. 
                """
            }
	    }

        stage ('Test')
        {
            steps
            {
                sh """ 
                    cd src
                    dotnet test BurnSystems.Tests/bin/Debug/net5.0/BurnSystems.Tests.dll --logger "trx;LogFileName=test.trx"
                    cd .. 
                """  

                mstest()
            }
            
        }
    }
}