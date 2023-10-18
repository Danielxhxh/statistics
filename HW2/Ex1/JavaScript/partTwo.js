const fs = require("fs");
const tsvData = fs.readFileSync("Professional Life - Sheet1.tsv", "utf8");
const rows = tsvData.split("\n");
const headers = rows[0].split("\t");
const excelData = [];

for (let i = 1; i < rows.length; i++) {
  const row = rows[i].split("\t");
  if (row.length === headers.length) {
    const jsonRow = {};

    for (let j = 0; j < headers.length; j++) {
      jsonRow[headers[j]] = row[j];
    }
    excelData.push(jsonRow);
  }
}

let variableA = "Age";
let variableB = "Ambitious (0-5)";

let matrix = jointDistribution(variableA, variableB);
printMatrix(matrix);

function jointDistribution(varA, varB) {
  let arrayA = getColumn(variableA);
  let arrayB = getColumn(variableB);

  let matrix = [];
  let header = ["-", ...arrayB];
  matrix.push(header);

  for (var i = 0; i < arrayA.length; i++) {
    let arr = new Array(arrayB.length + 1).fill(0);
    arr[0] = arrayA[i];

    excelData.forEach((element) => {
      if (element[varA] == arr[0]) {
        arr[header.indexOf(element[varB])]++;
      }
    });
    matrix.push(arr);
  }
  return matrix;
}

function getColumn(variable) {
  let arr = [];
  excelData.forEach((element) => {
    if (element[variable] !== undefined && !arr.includes(element[variable])) {
      arr.push(element[variable]);
    }
  });
  return arr.sort();
}

function printMatrix() {
  for (let i = 0; i < matrix.length; i++) {
    const row = matrix[i];
    let rowString = "";

    for (let j = 0; j < row.length; j++) {
      rowString += row[j] + "\t"; // Use '\t' for tab spacing
    }
    console.log(rowString);
  }
}
