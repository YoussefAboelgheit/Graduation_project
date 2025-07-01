
const modal = document.getElementById('editModal');
const editBtn = document.getElementById('editBtn');
const closeBtn = document.querySelector('.close-btn');

editBtn.addEventListener('click', function() {
modal.style.display = 'block';
});

closeBtn.addEventListener('click', function() {
modal.style.display = 'none';
});

window.addEventListener('click', function(event) {
if (event.target === modal) {
    modal.style.display = 'none';
}
});




// ******************************************************************************table*********************************
const rows = [
    {
    firstName: "Joe",
    lastName: "Blogs",
    level: "Intermediate",
    age: 21,
},
  // المزيد من الصفوف
{
    firstName: "mmm",
    lastName: "Blogs",
    level: "Intermediate",
    age: 77,
},
{
    firstName: "zzz",
    lastName: "Blogs",
    level: "Intermediate",
    age: 50,
},
{
    firstName: "Jaaa",
    lastName: "Blogs",
    level: "Intermediate",
    age: 30,
},

];

let sortOrder = "";
let sortField = "firstName";
let prevField = "firstName";

const sort = (arr, field, element) => {
sortField = field;
sortOrder = prevField === field && sortOrder === "asc" ? "desc" : "asc";
prevField = field;

  // إزالة التنسيق السابق من العناوين
document.querySelectorAll("thead th").forEach(th => {
    th.classList.remove("active");
    th.querySelector("i").className = "";
});

  // إضافة التنسيق للعنصر المحدد
if (element) {
    element.classList.add("active");
    element.querySelector("i").className = `fa-solid fa-arrow-${sortOrder === "asc" ? "up" : "down"}`;
}

  // فرز الصفوف
return arr.sort((a, b) => {
    const [valA, valB] = [a[field], b[field]];
    return typeof valA === typeof valB
    ? sortOrder === "asc"
        ? valA > valB ? 1 : -1
        : valA < valB ? 1 : -1
    : 0;
});
};

const sortRows = (field, element) => {
const tbody = document.querySelector("tbody");
tbody.innerHTML = sort(rows, field, element)
    .map(row => `
    <tr>
        <td>${row.firstName}</td>
        <td>${row.lastName}</td>
        <td>${row.level}</td>
        <td>${row.age}</td>
    </tr>
    `)
    .join("");
};

// تنفيذ الفرز الافتراضي عند تحميل الصفحة
sortRows("firstName", document.querySelector("thead th:first-child"));
