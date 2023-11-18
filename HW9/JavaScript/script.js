"use script";

const canvas1 = document.getElementById("canvas1");
const myCanvas = document.getElementById("myCanvas");
const chartID = "myCanvas";
const ctx = myCanvas.getContext("2d");
let canvasArr = [];

function destroyCanvas() {
  for (let c of canvasArr) {
    console.log(c);
    c.destroy();
  }
}

document.getElementById("SDE").addEventListener("change", function () {
  destroyCanvas();
  var selectedAlg = this.value;

  // Hide unused form
  var inputForms = document.getElementsByClassName("input-form");
  for (var i = 0; i < inputForms.length; i++) {
    inputForms[i].style.display = "none";
  }

  switch (selectedAlg) {
    case "empty":
      console.log("No algorithm selected");
      break;

    case "AB":
      console.log("Arithmetic Brownian selected");
      document.getElementById("ABInputs").style.display = "block";
      break;

    case "GB":
      console.log("Geometric Brownian selected");
      document.getElementById("GBInputs").style.display = "block";
      break;

    case "OU":
      console.log("Ornstein-Uhlenbeck selected");
      document.getElementById("OUInputs").style.display = "block";
      break;

    case "V":
      console.log("Vasicek selected");
      document.getElementById("VInputs").style.display = "block";
      break;

    case "HW":
      console.log("Hull-White selected");
      document.getElementById("HWInputs").style.display = "block";
      break;

    case "CIR":
      console.log("Cox-Ingersoll-Ross selected");
      document.getElementById("CIRInputs").style.display = "block";
      break;

    case "BK":
      console.log("Black-Karasinski selected");
      document.getElementById("BKInputs").style.display = "block";
      break;

    case "H":
      console.log("Heston selected");
      document.getElementById("HInputs").style.display = "block";
      break;

    case "CM":
      console.log("Chen model selected");
      // Add your code for Chen model here
      break;
  }
});

// Move canvas 1
myCanvas.addEventListener("mousedown", () => {
  myCanvas.addEventListener("mousemove", update1);
  window.addEventListener("mouseup", () => {
    myCanvas.removeEventListener("mousemove", update1);
  });
});

function update1(ev) {
  canvas1.style.setProperty("left", `${ev.x - 200}px`);
  canvas1.style.setProperty("top", `${ev.y - 25}px`);
}

function getRandomRGBAColor() {
  const r = Math.floor(Math.random() * 256);
  const g = Math.floor(Math.random() * 256);
  const b = Math.floor(Math.random() * 256);
  const a = Math.random();
  return `rgba(${r}, ${g}, ${b}, ${a})`;
}

// Function to generate Arithmetic Brownian Motion data
function generateArithmeticBrownianMotion() {
  destroyCanvas();

  const numSteps = parseInt(document.getElementById("ABnumSteps").value);
  const mu = parseFloat(document.getElementById("ABmu").value);
  const sigma = parseFloat(document.getElementById("ABsigma").value);
  const X0 = parseInt(document.getElementById("ABX0").value);
  const dt = parseFloat(document.getElementById("ABdt").value);

  const xValues = Array.from({ length: numSteps }, (_, i) => i);
  const yValues = [X0];

  for (let i = 0; i < numSteps; i++) {
    const dW = Math.sqrt(dt) * normalDistribution();
    const newValue = mu * dt + sigma * dW;
    yValues.push(yValues[i] + newValue);
  }

  let labelGraph = "Arithmetic Brownian Motion";
  drawGraph(xValues, yValues, labelGraph);
}

// Function to generate Geometric Brownian Motion data
function generateGeometricBrownianMotion() {
  destroyCanvas();

  const numSteps = parseInt(document.getElementById("GBnumSteps").value);
  const mu = parseFloat(document.getElementById("GBmu").value);
  const sigma = parseFloat(document.getElementById("GBsigma").value);
  const S0 = parseInt(document.getElementById("GBS0").value);
  const dt = parseFloat(document.getElementById("GBdt").value);

  const xValues = Array.from({ length: numSteps }, (_, i) => i);
  const yValues = [S0];

  for (let i = 0; i < numSteps; i++) {
    const dW = Math.sqrt(dt) * normalDistribution();
    const newValue = mu * yValues[i] * dt + sigma * yValues[i] * dW;
    yValues.push(yValues[i] + newValue);
  }
  let labelGraph = "Geometric Brownian Motion";
  drawGraph(xValues, yValues, labelGraph);
}

// Function to generate Ornstein-Uhlenbeck
function generateOU() {
  destroyCanvas();

  const numSteps = parseInt(document.getElementById("OUnumSteps").value);
  const theta = parseFloat(document.getElementById("OUtheta").value);
  const mu = parseFloat(document.getElementById("OUmu").value);
  const sigma = parseFloat(document.getElementById("OUsigma").value);
  const X0 = parseInt(document.getElementById("OUX0").value);
  const dt = parseFloat(document.getElementById("OUdt").value);

  let yValues = [X0];

  for (let i = 0; i < numSteps; i++) {
    const dW = Math.sqrt(dt) * normalDistribution();
    const k = theta * (mu - yValues[i]) * dt + sigma * dW;
    yValues.push(yValues[i] + k);
  }
  const xValues = Array.from({ length: numSteps }, (_, i) => i);
  drawGraph(xValues, yValues, "Ornstein-Uhlenbeck");
}

// Function to generate Vasicek
function generateVasicek() {
  destroyCanvas();

  const numSteps = parseInt(document.getElementById("VnumSteps").value);
  const theta = parseFloat(document.getElementById("Vtheta").value);
  const k = parseFloat(document.getElementById("Vk").value);
  const sigma = parseFloat(document.getElementById("Vsigma").value);
  const R0 = parseInt(document.getElementById("VR0").value);
  const dt = parseFloat(document.getElementById("Vdt").value);
  let yValues = [R0];

  for (let i = 0; i < numSteps; i++) {
    const dW = Math.sqrt(dt) * normalDistribution();
    const rt = k * (theta - yValues[i]) * dt + sigma * Math.sqrt(dt) * dW;
    yValues.push(yValues[i] + rt);
  }
  const xValues = Array.from({ length: numSteps }, (_, i) => i);
  drawGraph(xValues, yValues, "Vasicek");
}

// Function to generate Hull-White
function generateHullWhite() {
  destroyCanvas();

  const numSteps = parseInt(document.getElementById("hwNumSteps").value);
  const theta1 = parseFloat(document.getElementById("hwTheta1").value);
  const theta2 = parseFloat(document.getElementById("HWTheta2").value);
  const a = parseFloat(document.getElementById("hwA").value);
  const sigma = parseFloat(document.getElementById("hwSigma").value);
  const R0 = parseFloat(document.getElementById("hwR0").value);
  const dt = parseFloat(document.getElementById("hwDt").value);
  let yValues = [R0];

  for (let i = 0; i < numSteps; i++) {
    const dW = Math.sqrt(dt) * normalDistribution();
    const k = (theta1 + theta2 * i - a * yValues[i]) * dt + sigma * dW;
    yValues.push(yValues[i] + k);
  }
  const xValues = Array.from({ length: numSteps }, (_, i) => i);
  drawGraph(xValues, yValues, "Hull-White");
}

// Function to generate Cox-Ingersoll-Ross
function generateCoxIngersollRoss() {
  destroyCanvas();

  const numSteps = parseInt(document.getElementById("CIRNumSteps").value);
  const k = parseFloat(document.getElementById("CIRK").value);
  const theta = parseFloat(document.getElementById("CIRTheta").value);
  const sigma = parseFloat(document.getElementById("CIRSigma").value);
  const R0 = parseFloat(document.getElementById("CIRR0").value);
  const dt = parseFloat(document.getElementById("CIRdt").value);
  let yValues = [R0];

  for (let i = 0; i < numSteps; i++) {
    const dW = Math.sqrt(dt) * normalDistribution();
    const res =
      k * (theta - yValues[i]) * dt + sigma * Math.sqrt(yValues[i]) * dW;
    yValues.push(yValues[i] + res);
  }
  const xValues = Array.from({ length: numSteps }, (_, i) => i);
  drawGraph(xValues, yValues, "Cox-Ingersoll-Ross");
}

// Function to generate Black-Karasinski
function generateBlackKarasinski() {
  destroyCanvas();

  const numSteps = parseInt(document.getElementById("bkNumSteps").value);
  const theta1 = parseFloat(document.getElementById("BKTheta1").value);
  const theta2 = parseFloat(document.getElementById("BKTheta2").value);
  const a = parseFloat(document.getElementById("BKA").value);
  const sigma = parseFloat(document.getElementById("BKSigma").value);
  const R0 = parseFloat(document.getElementById("BKR0").value);
  const dt = parseFloat(document.getElementById("BKDt").value);
  let yValues = [R0];

  for (let i = 0; i < numSteps; i++) {
    const dW = Math.sqrt(dt) * normalDistribution();
    const res =
      (theta1 + theta2 * i - a * Math.log(yValues[i])) * dt +
      sigma * Math.sqrt(yValues[i]) * dW;
    yValues.push(yValues[i] + res);
  }
  const xValues = Array.from({ length: numSteps }, (_, i) => i);
  drawGraph(xValues, yValues, "Black-Karasinski");
}

// Support function for Heston (Get Cox-Ingersoll-Ross value)

// Function to generate Heston
function generateHeston() {
  destroyCanvas();

  const numSteps = parseInt(document.getElementById("hNumSteps").value);
  const mu = parseFloat(document.getElementById("HMu").value);
  const k = parseFloat(document.getElementById("HK").value);
  const theta = parseFloat(document.getElementById("HTheta").value);
  const sigma = parseFloat(document.getElementById("HSigma").value);
  const S0 = parseFloat(document.getElementById("HS0").value);
  const v0 = parseFloat(document.getElementById("HV0").value);
  const dt = parseFloat(document.getElementById("HDt").value);
  let yValues = [S0];
  let v_t = [v0];
  for (let i = 0; i < numSteps; i++) {
    const dW1 = Math.sqrt(dt) * normalDistribution();
    v_t.push(k * (theta - v_t[i]) * dt + sigma * Math.sqrt(v_t[i]) * dW1);
    const dW2 = Math.sqrt(dt) * normalDistribution();
    const S_t = mu * yValues[i] * dt + Math.sqrt(v_t[i]) * yValues[i] * dW2;
    yValues.push(yValues[i] + S_t);
  }
  const xValues = Array.from({ length: numSteps }, (_, i) => i);
  drawGraph(xValues, yValues, "Heston");
}

// Function to generate general stocastics process, it takes as input a,b,X0,dt, T and can process any EDS
function stochasticEulerMethod(a, b, X0, dt, labelGraph) {
  let numSteps = 100;
  let yValues = [X0];

  for (let i = 0; i < numSteps; i++) {
    const dW = Math.sqrt(dt) * normalDistribution();
    const k = a * (b - yValues[i]) * dt + sigma * dW; // theta * (mu - X[i]) * dt + sigma * dW;
    yValues.push(yValues[i] + k);
  }
  const xValues = Array.from({ length: numSteps }, (_, i) => i);
  drawGraph(xValues, yValues, labelGraph);
}

function normalDistribution() {
  let u = 0,
    v = 0;
  while (u === 0) u = Math.random(); // Convert [0,1) to (0,1)
  while (v === 0) v = Math.random();
  return Math.sqrt(-2.0 * Math.log(u)) * Math.cos(2.0 * Math.PI * v);
}

// Function used to draw graph
function drawGraph(xValues, yValues, labelGraph) {
  ctx.clearRect(0, 0, chartID.width, chartID.height);
  const myChart = new Chart(chartID, {
    type: "line",
    data: {
      labels: xValues,
      datasets: [
        {
          label: labelGraph,
          data: yValues,
          borderColor: "green",
          fill: false,
        },
      ],
    },
    options: {
      responsive: true,
      elements: {
        line: {
          tension: 0.1,
        },
      },
      legend: {
        display: false,
      },
      tooltips: {
        enabled: false,
      },
    },
  });
  canvasArr.push(myChart);
}
