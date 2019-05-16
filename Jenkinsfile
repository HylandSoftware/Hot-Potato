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
        
		stage("NuGet-Restore") {
            steps {
                container("builder") {
                    // sh 'chmod +x ./build.sh'
                    // sh './build.sh -t NuGet-Restore'
                    sh 'dotnet restore'
                }
            }
        }

        stage("Build") {
            steps {
                container("builder") {
                    //sh './build.sh -t Build'
                    sh 'dotnet build --no-restore'
                }
            }
        }

        stage("Run-Unit-Tests") {
            steps {
                container("builder") {
                    //sh './build.sh -t Run-Unit-Tests'
                    sh 'dotnet test '
                }
            }
        }
		
		stage("Run-Integration-Tests") {
            steps {
                container("builder") {
                    sh './build.sh -t Run-Integration-Tests'
                }
            }
        }

        stage("Run-E2E-Tests") {
            steps {
                container("builder") {
                    sh './build.sh -t Run-E2E-Tests'
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