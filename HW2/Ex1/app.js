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

let Instruments = AbsoluteQualitative();
let Ambitious = AbsoluteQuantitativeDiscrete();
let Weight = AbsoluteQuantitativeContinuous();

// console.log("Absolute frequency of Instruments: ", Instruments.data);
// console.log("Absolute frequency of Ambituous: ", Ambitious.data);
// console.log("Absolute frequency of Weight: ", Weight.data);

function AbsoluteQualitative() {
  // Qualitative: Instruments
  let data = {};
  let question = "Play some instruments? Which ones?";
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
  return { data, total };
}

function AbsoluteQuantitativeDiscrete() {
  // Quantitative discrete: Ambitious (in a scale from 0 to 5)
  let data = { 1: 0, 2: 0, 3: 0, 4: 0, 5: 0 };
  let question = "Ambitious (0-5)";
  let total = 0;

  // Save all the data in a JSON
  excelData.forEach((element) => {
    if (element[question] !== undefined) {
      let a = element[question];
      if (a !== undefined && a > 0 && a < 6) {
        data[a] += 1;
        total++;
      }
    }
  });

  return { data, total };
}

function AbsoluteQuantitativeContinuous() {
  // Quantitative continuous: Weight
  let data = {
    "50-": 0,
    "[50;60)": 0,
    "[60;70)": 0,
    "[70;80)": 0,
    "80+": 0,
  };
  let question = "weight";
  let total = 0;

  // Save all data in a JSON
  excelData.forEach((element) => {
    if (element[question] !== undefined) {
      let w = element[question];
      total++;
      if (w < 50) {
        data["50-"] += 1;
      } else if (w >= 50 && w < 60) {
        data["[50;60)"] += 1;
      } else if (w >= 60 && w < 70) {
        data["[60;70)"] += 1;
      } else if (w >= 70 && w < 80) {
        data["[70;80)"] += 1;
      } else if (w >= 80) {
        data["80+"] += 1;
      }
    }
  });
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
let PercentageInstruments = Percentage(Instruments.data, Instruments.total);
let PercentageAmbitious = Percentage(Ambitious.data, Ambitious.total);
let PercentageWeight = Percentage(Weight.data, Weight.total);

// console.log("Percentage frequency of Instruments: ", PercentageInstruments);
// console.log("Percentage frequency of Ambitious: ", PercentageAmbitious);
// console.log("Percentage frequency of Weight: ", PercentageWeight);

function Percentage(data, total) {
  for (let key in data) {
    data[key] = `${(data[key] * 100).toFixed(2)} %`;
  }
  return data;
}
