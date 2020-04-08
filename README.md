# Trend Micro -> DataDog (Events)
Simple lambda function to take events posted to SNS from TrendMicro and post them as Datadog Events.

## Installation
1. Create an SNS Topic in your AWS account
2. Create a Lambda function using the `.zip` found in [Releases](https://github.com/nwestfall/TrendMicroToDatadog/releases) (or clone the repo, make changes, and publish)
   - Runtime: dotnetcore3.1
   - Handler: `TrendMicroToDatadog::TrendMicroToDatadog.Function::FunctionHandler`
3. Setup the Lambda function as a subscriber to the topic
4. Add `DATADOG_URL` and `DATADOG_API_KEY` as Environment Variables in your Lambda Function
5. Setup Trend Micro to send events to SNS - [https://help.deepsecurity.trendmicro.com/event-sns.html](https://help.deepsecurity.trendmicro.com/event-sns.html)
6. Watch as events come to Datadog!

## Event Types
I don't have a full knowledge of all the event types that come over, but want to build support for as many as possible.  If you get an event type that you think can be enhanced, create an issue with the template for "New Event Type Support".  If possible, I would really need the JSON sent to SNS so I can see what Trend Micro is sending and add it to the tests.

## Here are some steps to follow to get started from the command line (if building yourself)

Once you have edited your template and code you can deploy your application using the [Amazon.Lambda.Tools Global Tool](https://github.com/aws/aws-extensions-for-dotnet-cli#aws-lambda-amazonlambdatools) from the command line.

Install Amazon.Lambda.Tools Global Tools if not already installed.
```
    dotnet tool install -g Amazon.Lambda.Tools
```

If already installed check if new version is available.
```
    dotnet tool update -g Amazon.Lambda.Tools
```

Execute unit tests
```
    cd "TrendMicroToDatadog/test/TrendMicroToDatadog.Tests"
    dotnet test
```

Deploy function to AWS Lambda
```
    cd "TrendMicroToDatadog/src/TrendMicroToDatadog"
    dotnet lambda deploy-function
```
