/**
 * Practical 10 - Student Management System
 * Simple Student Project JavaScript (CRUD operations)
 */

// Fresh, realistic dummy data for student project
const dummyStudents = [
  { id: 1, name: "Aarav Sharma", email: "aarav.sharma@gmail.com", course: "Computer Engineering", age: 21 },
  { id: 2, name: "Diya Mehta", email: "diya.mehta@gmail.com", course: "Information Technology", age: 20 },
  { id: 3, name: "Rohan Verma", email: "rohan.verma@gmail.com", course: "Mechanical Engineering", age: 22 },
  { id: 4, name: "Sneha Kulkarni", email: "sneha.k@gmail.com", course: "Electrical Engineering", age: 21 }
];

let studentList = [];
let isEditing = false;

// DOM Elements
const studentForm = document.getElementById('studentForm');
const formHeading = document.getElementById('formHeading');
const submitBtn = document.getElementById('submitBtn');
const resetBtn = document.getElementById('resetBtn');
const studentIdInput = document.getElementById('studentId');
const nameInput = document.getElementById('name');
const emailInput = document.getElementById('email');
const courseSelect = document.getElementById('course');
const ageInput = document.getElementById('age');
const tableBody = document.getElementById('studentTableBody');
const noDataRow = document.getElementById('noDataRow');
const searchInput = document.getElementById('searchInput');
const recordCount = document.getElementById('recordCount');
const alertBox = document.getElementById('alertBox');

// On Page Load
document.addEventListener('DOMContentLoaded', () => {
  loadStudents();

  // Form submit (Add or Update)
  studentForm.addEventListener('submit', handleFormSubmit);

  // Reset form
  resetBtn.addEventListener('click', resetForm);

  // Search filter
  searchInput.addEventListener('input', renderTable);
});

// Load students from API (or fallback to dummy data)
async function loadStudents() {
  try {
    const res = await fetch('/api/students');
    if (!res.ok) throw new Error();
    studentList = await res.json();
  } catch (e) {
    // If backend API isn't running or during static preview, use dummy data
    studentList = [...dummyStudents];
  }
  renderTable();
}

// Render Table
function renderTable() {
  const filter = searchInput.value.toLowerCase().trim();
  const filtered = studentList.filter(s =>
    s.name.toLowerCase().includes(filter) ||
    s.course.toLowerCase().includes(filter) ||
    s.email.toLowerCase().includes(filter)
  );

  recordCount.textContent = filtered.length;
  tableBody.innerHTML = '';

  if (filtered.length === 0) {
    noDataRow.style.display = 'block';
    return;
  }

  noDataRow.style.display = 'none';

  filtered.forEach(student => {
    const row = document.createElement('tr');
    row.innerHTML = `
      <td>${student.id}</td>
      <td><strong>${escapeHtml(student.name)}</strong></td>
      <td>${escapeHtml(student.email)}</td>
      <td>${escapeHtml(student.course)}</td>
      <td>${student.age}</td>
      <td style="text-align: center;">
        <button class="btn-sm btn-edit" onclick="editStudent(${student.id})">Edit</button>
        <button class="btn-sm btn-delete" onclick="deleteStudent(${student.id})">Delete</button>
      </td>
    `;
    tableBody.appendChild(row);
  });
}

// Handle Add / Update
async function handleFormSubmit(e) {
  e.preventDefault();

  const name = nameInput.value.trim();
  const email = emailInput.value.trim();
  const course = courseSelect.value;
  const age = parseInt(ageInput.value, 10);

  if (!name || !email || !course || isNaN(age)) {
    showAlert('Please fill in all required fields.', 'danger');
    return;
  }

  if (isEditing) {
    // Update existing student
    const id = parseInt(studentIdInput.value, 10);
    const updatedData = { id, name, email, course, age };

    try {
      await fetch(`/api/students/${id}`, {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(updatedData)
      });
    } catch (err) {
      // offline fallback
    }

    const index = studentList.findIndex(s => s.id === id);
    if (index !== -1) {
      studentList[index] = updatedData;
    }
    showAlert(`Student "${name}" updated successfully!`, 'success');
  } else {
    // Add new student
    const newId = studentList.length > 0 ? Math.max(...studentList.map(s => s.id)) + 1 : 1;
    const newStudent = { id: newId, name, email, course, age };

    try {
      await fetch('/api/students', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(newStudent)
      });
    } catch (err) {
      // offline fallback
    }

    studentList.push(newStudent);
    showAlert(`Student "${name}" added successfully!`, 'success');
  }

  resetForm();
  renderTable();
}

// Edit button click
window.editStudent = function (id) {
  const student = studentList.find(s => s.id === id);
  if (!student) return;

  isEditing = true;
  formHeading.textContent = 'Edit Student (ID: ' + student.id + ')';
  submitBtn.textContent = 'Update Student';
  submitBtn.className = 'btn btn-primary';

  studentIdInput.value = student.id;
  nameInput.value = student.name;
  emailInput.value = student.email;
  courseSelect.value = student.course;
  ageInput.value = student.age;

  nameInput.focus();
};

// Delete button click
window.deleteStudent = async function (id) {
  const student = studentList.find(s => s.id === id);
  const name = student ? student.name : `#${id}`;

  if (!confirm(`Are you sure you want to delete student "${name}"?`)) {
    return;
  }

  try {
    await fetch(`/api/students/${id}`, { method: 'DELETE' });
  } catch (err) {
    // offline fallback
  }

  studentList = studentList.filter(s => s.id !== id);
  showAlert(`Student "${name}" deleted.`, 'danger');

  if (isEditing && parseInt(studentIdInput.value, 10) === id) {
    resetForm();
  }

  renderTable();
};

// Reset Form
function resetForm() {
  isEditing = false;
  formHeading.textContent = 'Add New Student';
  submitBtn.textContent = 'Add Student';
  studentForm.reset();
  studentIdInput.value = '';
}

// Show Alert
function showAlert(message, type) {
  alertBox.className = `alert-box alert-${type}`;
  alertBox.textContent = message;
  alertBox.style.display = 'block';

  setTimeout(() => {
    alertBox.style.display = 'none';
  }, 3000);
}

// Simple HTML escaper
function escapeHtml(str) {
  if (!str) return '';
  return str.replace(/[&<>"']/g, m => ({
    '&': '&amp;', '<': '&lt;', '>': '&gt;', '"': '&quot;', "'": '&#039;'
  })[m]);
}
