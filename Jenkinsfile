pipeline {
    agent {
        kubernetes {
            label "hot-potato-${UUID.randomUUID().toString()}"
            yamlFile './build-spec.yml'
        }
    }

    stages {
        stage('Version') {
            environment {
                IGNORE_NORMALISATION_GIT_HEAD_MOVE = "1"
            }
            steps {
                container('gitversion') {
                    script {
                        env.IMAGE_VERSION = sh(script: 'mono /usr/lib/GitVersion/GitVersion.exe /output json /showvariable MajorMinorPatch', returnStdout: true).trim()
                        echo IMAGE_VERSION 
                    }
                }
            }
        }

        stage("Build") {
            steps {
                container("docker") {
                    sh 'docker build --target build --tag hcr.io/hotpotato:build --build-arg IMAGE_VERSION=${IMAGE_VERSION} .'
                    //sh 'dotnet build --configuration Release -p:Version=${IMAGE_VERSION}'
                }
            }
        }

        stage("Run-Unit-Tests") {
            steps {
                container("docker") {
                    sh 'docker build --target test --tag hcr.io/hotpotato:test --build-arg IMAGE_VERSION=${IMAGE_VERSION} .'
                    sh 'docker run -v $WORKSPACE/test/results:/app/test/results -v $WORKSPACE/test/coverage:/app/test/coverage hcr.io/hotpotato:test'

                    // sh 'dotnet test ./test/HotPotato.Core.Test/HotPotato.Core.Test.csproj -c Release -p:CollectCoverage=true -p:CoverletOutputFormat=cobertura -p:CoverletOutput=./test/coverage/coreCoverage.xml -p:Exclude="[xunit.*]*" -l:"JUnit;LogFilePath=$WORKSPACE/test/results/coreResults.xml" --no-restore --no-build'
                    // sh 'dotnet test ./test/HotPotato.AspNetCore.Middleware.Test/HotPotato.AspNetCore.Middleware.Test.csproj -c Release -p:CollectCoverage=true -p:CoverletOutputFormat=cobertura -p:CoverletOutput=./test/coverage/middlewareCoverage.xml -p:Include="[*.Middleware]*" -l:"JUnit;LogFilePath=$WORKSPACE/test/results/middlewareResults.xml" --no-restore --no-build'
                    // sh 'dotnet test ./test/HotPotato.OpenApi.Test/HotPotato.OpenApi.Test.csproj -c Release -p:CollectCoverage=true -p:CoverletOutputFormat=cobertura -p:CoverletOutput=./test/coverage/openApiCoverage.xml -p:Include="[*.OpenApi]*" -l:"JUnit;LogFilePath=$WORKSPACE/test/results/openapiResults.xml" --no-restore --no-build'
                }
            }
        }
		
		// stage("Run-Integration-Tests") {
        //     steps {
        //         container("builder") {
        //             sh 'dotnet test ./test/HotPotato.Integration.Test/HotPotato.Integration.Test.csproj -c Release -l:"JUnit;LogFilePath=$WORKSPACE/test/results/integrationResults.xml" --no-restore --no-build'
        //         }
        //     }
        // }

        // stage("Run-E2E-Tests") {
        //     steps {
        //         container("builder") {
        //             sh 'dotnet test ./test/HotPotato.E2E.Test/HotPotato.E2E.Test.csproj -c Release -l:"JUnit;LogFilePath=$WORKSPACE/test/results/E2EResults.xml" --no-restore --no-build'

        //             sh 'dotnet $WORKSPACE/src/HotPotato.AspNetCore.Host/bin/Release/netcoreapp2.1/HotPotato.AspNetCore.Host.dll &'
        //             sh 'dotnet $WORKSPACE/test/HotPotato.Api/bin/Release/netcoreapp2.1/HotPotato.Api.dll &'
        //         }
        //         container("newman") {
        //             sh 'newman run $WORKSPACE/test/HappyPathTests.postman_collection.json'
        //             sh 'newman run $WORKSPACE/test/Non-ConformantTests.postman_collection.json'
        //             sh 'newman run $WORKSPACE/test/NotInSpecTests.postman_collection.json'
        //         }
        //     }
        // }

        stage("Deploy") {
            when {
                branch 'master'
            }
            environment {
                API_KEY = credentials('testautomation_push')
            }
            steps {
                container("builder") {
                    sh 'dotnet pack ./src/HotPotato.AspNetCore.Host/HotPotato.AspNetCore.Host.csproj -p:PackageVersion=${IMAGE_VERSION} -c Release --no-build --no-restore'
                    sh 'dotnet pack ./src/HotPotato.AspNetCore.Middleware/HotPotato.AspNetCore.Middleware.csproj -p:PackageVersion=${IMAGE_VERSION} -c Release --no-build --no-restore'
                    sh 'dotnet pack ./src/HotPotato.Core/HotPotato.Core.csproj -p:PackageVersion=${IMAGE_VERSION} -c Release --no-build --no-restore'
                    sh 'dotnet pack ./src/HotPotato.OpenApi/HotPotato.OpenApi.csproj -p:PackageVersion=${IMAGE_VERSION} -c Release --no-build --no-restore'

                    sh 'dotnet nuget push $WORKSPACE/src/HotPotato.AspNetCore.Host/**/*.nupkg -k ${API_KEY} -s https://proget.onbase.net/nuget/NuGet/'
                    sh 'dotnet nuget push $WORKSPACE/src/HotPotato.AspNetCore.Middleware/**/*.nupkg -k ${API_KEY} -s https://proget.onbase.net/nuget/NuGet/'
                    sh 'dotnet nuget push $WORKSPACE/src/HotPotato.Core/**/*.nupkg -k ${API_KEY} -s https://proget.onbase.net/nuget/NuGet/'
                    sh 'dotnet nuget push $WORKSPACE/src/HotPotato.OpenApi/**/*.nupkg -k ${API_KEY} -s https://proget.onbase.net/nuget/NuGet/'
                }
            }
        }
    }
    post {
        always {
            junit '$WORKSPACE/test/results/*.xml'
            cobertura coberturaReportFile: '$WORKSPACE/test/coverage/*.xml'
        }
        regression {
            mattermostSend color: "#ef1717", icon: "https://jenkins.io/images/logos/jenkins/jenkins.png", message: "Someone broke ${env.BRANCH_NAME}, Ref build number -- ${env.BUILD_NUMBER}! (<${env.BUILD_URL}|${env.BUILD_URL}>)"
        }

        fixed {
            mattermostSend color: "#7FFF00", icon: "https://jenkins.io/images/logos/jenkins/jenkins.png", message: "Someone fixed ${env.BRANCH_NAME}, Ref build number -- ${env.BUILD_NUMBER}! (<${env.BUILD_URL}|${env.BUILD_URL}>)"
        }
    }
}