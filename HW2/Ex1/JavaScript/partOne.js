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

let Instruments = Absolute("Play some instruments? Which ones?");
let Ambitious = Absolute("Ambitious (0-5)");
let Weight = AbsoluteQuantitativeContinuous(5, "weight");

// console.log("Absolute frequency of Instruments: ", Instruments.data);
// console.log("Absolute frequency of Ambituous: ", Ambitious.data);
// console.log("Absolute frequency of Weight: ", Weight.data);

function Absolute(question) {
  let data = {};
  let total = 0;

  // Save all the data in a JSON
  excelData.forEach((element) => {
    if (element[question] !== undefined) {
      let i = element[question].toLowerCase();
      total++;
      if (data[i] === undefined) {
        data[i] = 1;
      } else {
        data[i] += 1;
      }
    }
  });
  data = orderData(data);
  return { data, total };
}

function orderData(inputObject) {
  const sortedKeys = Object.keys(inputObject).sort(
    (a, b) => a.localeCompare(b) || Number(a) - Number(b)
  );

  return sortedKeys.reduce(
    (obj, key) => ({ ...obj, [key]: inputObject[key] }),
    {}
  );
}

function AbsoluteQuantitativeContinuous(numIntervals, question) {
  let data = {};
  let total = 0;

  let min = Number.POSITIVE_INFINITY;
  let max = Number.NEGATIVE_INFINITY;

  excelData.forEach((element) => {
    if (element[question] !== undefined) {
      let w = parseFloat(element[question]);
      if (w < min) {
        min = w;
      }
      if (w > max) {
        max = w;
      }
    }
  });
  let intervalSize = (max - min) / numIntervals;

  // Create the intervals and add in dictionary
  for (let i = 0; i < numIntervals; i++) {
    const start = min + i * intervalSize;
    const end = min + (i + 1) * intervalSize;
    data[`[${start};${end})`] = 0;
  }

  excelData.forEach((element) => {
    let w = parseFloat(element[question]);
    if (!isNaN(w)) {
      total++;
      for (let i = 0; i < numIntervals; i++) {
        const start = min + i * intervalSize;
        const end = min + (i + 1) * intervalSize;
        if (w >= start && w < end) {
          data[`[${start};${end})`] += 1;
          break;
        }
      }
    }
  });
  console.log(data, total);
  return { data, total };
}

// -------------------------------------------------

let RelativeInstruments = Relative(Instruments.data, Instruments.total);
let RelativeAmbitious = Relative(Ambitious.data, Ambitious.total);
let RelativeWeight = Relative(Weight.data, Weight.total);

// console.log("Relative frequency of Instruments: ", RelativeInstruments);
// console.log("Relative frequency of Ambitious: ", RelativeAmbitious);
// console.log("Relative frequency of Weight: ", RelativeWeight);

function Relative(data, total) {
  for (let key in data) {
    data[key] = data[key] / total;
  }
  return data;
}

// -------------------------------------------------
let PercentageInstruments = Percentage(RelativeInstruments);
let PercentageAmbitious = Percentage(RelativeAmbitious);
let PercentageWeight = Percentage(RelativeWeight);

// console.log("Percentage frequency of Instruments: ", PercentageInstruments);
// console.log("Percentage frequency of Ambitious: ", PercentageAmbitious);
// console.log("Percentage frequency of Weight: ", PercentageWeight);

function Percentage(relativeData) {
  for (let key in relativeData) {
    relativeData[key] = `${(relativeData[key] * 100).toFixed(2)} %`;
  }
  return relativeData;
}
