// **************************التقسيمه*********************
// الكود أدناه اختياري ويمكنك تعديله لاحقاً حسب متطلبات التنقل أو الوظائف الأخرى
document.addEventListener('DOMContentLoaded', () => {
    const jobItems = document.querySelectorAll('.job-item');
    jobItems.forEach(item => {
        item.addEventListener('click', (e) => {
            // يمكنك هنا التعامل مع التنقل أو إضافة وظائف أخرى
            console.log(`Clicked on: ${item.querySelector('span').textContent}`);
        });
    });
});
document.querySelector('.ie').addEventListener('click', function () {
    const governorate = document.getElementById('governorate-app').value;
    const experience = document.getElementById('experience').value;
    const job = document.getElementById('job').value;

    fetch(`/Home/FilterApplicants?governorate=${encodeURIComponent(governorate)}&experience=${encodeURIComponent(experience)}&job=${encodeURIComponent(job)}`)
        .then(response => response.text())
        .then(html => {
            document.getElementById('Applicants').innerHTML = html;
        });
});

// *********************advs***************************
// Removed the alert functionality - details buttons will now work normally
// document.addEventListener('DOMContentLoaded', function() {
//     const detailButtons = document.querySelectorAll('.details-btn');

//     detailButtons.forEach(button => {
//       button.addEventListener('click', function() {
//         alert('More details will be displayed here!');
//       });
//     });
// });