/*
ARRAY --------------------------------------------------------------------------------------
*/
let array = [0, 1, 2, 3, 4, 5];

// Loop (continue/break)
array.forEach((element) => {
  //   console.log(element);
});

for (let i = 0; i < array.length; i++) {
  if (array[i] === 2) {
    continue;
  } else if (array[i] === 4) {
    break;
  }
  //   console.log(array[i]);
}

// Other operations

array.push(3); // Add
array.pop(); // Remove last element
array[2]; // Get
array[2] = "Ciao"; // Set
array.includes("Ciao"); // Check existence (returns true/false)
array.sort(); // Sort

/*
DICTIONARY --------------------------------------------------------------------------------------
*/
let dic = {
  key1: "val1",
  key2: "val2",
  key3: "val3",
};

// Loop
const entries = Object.entries(dic); // Returns an array of key:value pairs

for (const [key, value] of entries) {
  console.log(key + ": " + value);
}

// Other operations
dic.key3 = "val3"; // Add

delete dic.key3; // Remove

let val1 = dic.key1; // Get

dic.key1 = "modified-val1"; // Set

// Check key
if (dic.key2 !== undefined) {
  console.log("- Key exists");
} else {
  console.log("- Key does not exist");
}

// Check value
const values = Object.values(dic);
if (values.includes("val2")) {
  console.log("- Value exists");
} else {
  console.log("- Value does not exist");
}

/*
HASHSET --------------------------------------------------------------------------------------
It is like an array but contains unique values
*/

const mySet = new Set();
mySet.add(1);
mySet.add(5);
mySet.add("text");
mySet.add({ a: 1, b: 2 });
mySet.add({ a: 1, b: 2 });

// Loop through the Set using a for...of loop
for (const element of mySet) {
  console.log(element);
}

mySet.add("new text"); // Add

mySet.delete(5); // Remove

mySet.has(1); // Check existence (returns true/false)

/*
QUEUE and STACK -----------
Both are implemented using an array, so they got the same other methods of arrays. 
*/

// Queue
const queue = ["Task 1", "Task 2"];

queue.push("Task 3"); // Add/Enqueue

const taskQueue = queue.shift(); // Remove/Dequeue

// Stack
const stack = ["Task 1", "Task 2"];

stack.push("Task 3"); // Add

const taskStack = stack.pop(); // Remove

/*
LINKED LIST --------------------------------------------------------------------------------------
*/

class Node {
  constructor(data) {
    this.data = data;
    this.next = null;
  }
}

class LinkedList {
  constructor() {
    this.head = null;
  }

  append(data) {
    const newNode = new Node(data);
    if (!this.head) {
      this.head = newNode;
    } else {
      let current = this.head;
      while (current.next) {
        current = current.next;
      }
      current.next = newNode;
    }
  }

  display() {
    let current = this.head;
    while (current) {
      console.log(current.data);
      current = current.next;
    }
  }
}

// Create a new linked list
const myList = new LinkedList();

// Append elements to the linked list
myList.append("Item 1");
myList.append("Item 2");
myList.append("Item 3");

// Display the lin
myList.display();
