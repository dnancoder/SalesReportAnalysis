# Sales Report Analysis

## Prerequisites
- These pre-requisite would help Running and Testing the Web App when you don't have Visual Studio
- Install [VS Code](https://code.visualstudio.com/).
- Clone the GitHub repository to your local machine.
- Install the following extensions in VS Code:
  1. [ .NET Install Tool](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.vscode-dotnet-runtime)
  2. [C#](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csharp)
  3. [C# Dev Kit](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csdevkit)
  4. [Rest Client](https://marketplace.visualstudio.com/items?itemName=humao.rest-client)


## Web API Operations

The Web API supports the following operations:

### 1. GET - GetSalesRecordSummary
- **Description**: Takes the input as a CSV file name. The file needs to be added to the "Data" folder in the project "ImplinjSalesAnalysis". If the file is not present, it will return the error "Could not find file".
- **URL**: `http://localhost:5064/api/SalesRecordAnalysis/GetSummary?fileName=SalesRecords.csv`

### 2. POST - UploadFileAndGetSalesRecordSummary
- **Description**: This POST API accepts the CSV file as an attachment. I have tested this using `ImpinjSalesAnalysis.http`. After running the project, you need to click on "Send Request".
- **URL**: `http://localhost:5064/api/SalesRecordAnalysis/PostSummary`
- **Note**: You need to send the attachment, tested using the `.http` file.

## Build and Run the Project

- **IDE**: Use Visual Studio Community Edition.
- **Notes**: 
  - Use HTTP to avoid any certificate issues on local while running the project.
  - After running the project, you can use `ImpinjSalesAnalysis.http` to trigger the HTTP calls.
  - The functionality has been validated using Unit tests.

## Usage

1. **Clone the Repository**:
   ```sh
   git clone <repository-url>






Web API supports below operations as described:
1. GET - GetSalesRecordSummary: Takes the input as CSV file name. File needs to be add to the "Data" folder in project "ImplinjSalesAnalysis". If file is not present it will return error "Could not find file"
	URL: http://localhost:5064/api/SalesRecordAnalysis/GetSummary?fileName=SalesRecords.csv
2. Post - UploadFileAndGetSalesRecordSummary: As part of the post API accesspts the CSV file as attachment. I have tested using ImplinjSalesAnalysis.http. After running the project, need to click on ""Send Request""
	URL: http://localhost:5064/api/SalesRecordAnalysis/PostSummary need to send attachment, tested using .http file.

Build and Run the project using Visual Studio Community edition.
Using http to avoid any cert issues on local while running the project.
After running the project you can use "ImplinjSalesAnalysis.http" to trigger the http calls.
Functionality has been validated using Unit tests as well.