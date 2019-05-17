pipeline {
    agent {
        kubernetes {
            label 'hot-potato'
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
                        env.IMAGE_VERSION = sh(script: 'mono /usr/lib/GitVersion/GitVersion.exe /output json /showvariable NuGetVersionV2', returnStdout: true).trim()
                        echo IMAGE_VERSION 
                    }
                }
            }
        }

        stage("Build") {
            steps {
                container("builder") {
                    sh 'dotnet build --configuration Release -p:Version=${IMAGE_VERSION}'
                }
            }
        }

        stage("Run-Unit-Tests") {
            steps {
                container("builder") {
                    sh 'dotnet test ./test/HotPotato.Core.Test/HotPotato.Core.Test.csproj --configuration Release -r core-test-results.xml --no-restore --no-build'
                    sh 'dotnet test ./test/HotPotato.AspNetCore.Middleware.Test/HotPotato.AspNetCore.Middleware.Test.csproj --configuration Release -r middleware-test-results.xml --no-restore --no-build'
                    sh 'dotnet test ./test/HotPotato.OpenApi.Test/HotPotato.OpenApi.Test.csproj --configuration Release -r openapi-test-results.xml --no-restore --no-build'
                }
            }
        }
		
		stage("Run-Integration-Tests") {
            steps {
                container("builder") {
                    sh 'dotnet test ./test/HotPotato.Integration.Test/HotPotato.Integration.Test.csproj --configuration Release -r integration-test-results.xml --no-restore --no-build'
                }
            }
        }

        stage("Run-E2E-Tests") {
            steps {
                container("builder") {
                    sh 'dotnet test ./test/HotPotato.E2E.Test/HotPotato.E2E.Test.csproj --configuration Release -r E2E-test-results.xml --no-restore --no-build'
                }
            }
        }

        stage("Deploy") {
            when {
                branch 'feat/AUTOTEST-326-deploy-hotpotato-packages-to-nuget' //'master'
            }
            environment {
                API_KEY = credentials('testautomation_push')
            }
            steps {
                container("builder") {
                    sh 'dotnet pack ./src/HotPotato.AspNetCore.Host/HotPotato.AspNetCore.Host.csproj -o $WORKSPACE/nupkg -c Release --no-build --no-restore'
                    sh 'dotnet pack ./src/HotPotato.AspNetCore.Middleware/HotPotato.AspNetCore.Middleware.csproj $WORKSPACE/nupkg -c Release --no-build --no-restore'
                    sh 'dotnet pack ./src/HotPotato.Core/HotPotato.Core.csproj $WORKSPACE/nupkg -c Release --no-build --no-restore'
                    sh 'dotnet pack ./src/HotPotato.OpenApi/HotPotato.OpenApi.csproj $WORKSPACE/nupkg -c Release --no-build --no-restore'

                    sh 'dotnet nuget push $WORKSPACE/nupkg/*.nupkg -k ${API_KEY} -s https://proget.onbase.net/nuget/TestFeed/' //https://proget.onbase.net/nuget/NuGet/
                }
            }
        }
    }
    post {
        regression {
            mattermostSend color: "#ef1717", icon: "https://jenkins.io/images/logos/jenkins/jenkins.png", message: "Someone broke ${env.BRANCH_NAME}, Ref build number -- ${env.BUILD_NUMBER}! (<${env.BUILD_URL}|${env.BUILD_URL}>)"
        }

        fixed {
            mattermostSend color: "#7FFF00", icon: "https://jenkins.io/images/logos/jenkins/jenkins.png", message: "Someone fixed ${env.BRANCH_NAME}, Ref build number -- ${env.BUILD_NUMBER}! (<${env.BUILD_URL}|${env.BUILD_URL}>)"
        }
    }
}