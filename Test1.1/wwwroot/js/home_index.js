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
document.querySelector('.aa').addEventListener('click', function () {
    const governorate = document.getElementById('governorate-ad').value;
    const job = document.getElementById('job-ad').value;
    const workType = document.getElementById('work-type').value; // THIS WAS MISSING!
    const salary = document.getElementById('salary').value;

    // Get container but don't show loading immediately
    const adsContainer = document.querySelector('.ads-container');

    // Include workType in the fetch URL
    fetch(`/Home/FilterAdvertisements?governorate=${encodeURIComponent(governorate)}&job=${encodeURIComponent(job)}&workType=${encodeURIComponent(workType)}&salary=${encodeURIComponent(salary)}`)
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.text();
        })
        .then(html => {
            adsContainer.innerHTML = html;
        })
        .catch(error => {
            console.error('Error filtering advertisements:', error);
            adsContainer.innerHTML = '<div class="error">Error loading results. Please try again.</div>';
        });
});