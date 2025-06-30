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

function setShowMoreFiltersApplicants(governorate, experience, job) {
    const form = document.querySelector('.show-more-applicants-form');
    if (form) {
        form.querySelector('input[name="governorate"]').value = governorate;
        form.querySelector('input[name="experience"]').value = experience;
        form.querySelector('input[name="job"]').value = job;
    }
}

function setShowMoreFiltersAds(governorate, job, salary) {
    const form = document.querySelector('.show-more-ads-form');
    if (form) {
        form.querySelector('input[name="governorate"]').value = governorate;
        form.querySelector('input[name="job"]').value = job;
        form.querySelector('input[name="salary"]').value = salary;
    }
}

// Update Applicants filter
const applyApplicantsBtn = document.querySelector('.ie');
if (applyApplicantsBtn) {
    applyApplicantsBtn.addEventListener('click', function () {
        const governorate = document.getElementById('governorate-app').value;
        const experience = document.getElementById('experience').value;
        const job = document.getElementById('job').value;

        fetch(`/Home/FilterApplicants?governorate=${encodeURIComponent(governorate)}&experience=${encodeURIComponent(experience)}&job=${encodeURIComponent(job)}`)
            .then(response => response.text())
            .then(html => {
                document.getElementById('Applicants').innerHTML = html;
                setShowMoreFiltersApplicants(governorate, experience, job);
            });
    });
}

// Update Company Ads filter
const applyAdsBtn = document.querySelector('.aa');
if (applyAdsBtn) {
    applyAdsBtn.addEventListener('click', function () {
        const governorate = document.getElementById('governorate-ad').value;
        const job = document.getElementById('job-ad').value;
        const salary = document.getElementById('salary').value;

        const adsContainer = document.querySelector('.ads-container');

        fetch(`/Home/FilterAdvertisements?governorate=${encodeURIComponent(governorate)}&job=${encodeURIComponent(job)}&salary=${encodeURIComponent(salary)}`)
            .then(response => {
                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }
                return response.text();
            })
            .then(html => {
                adsContainer.innerHTML = html;
                setShowMoreFiltersAds(governorate, job, salary);
            })
            .catch(error => {
                console.error('Error filtering advertisements:', error);
                adsContainer.innerHTML = '<div class="error">Error loading results. Please try again.</div>';
            });
    });
}

//document.querySelector('.aa').addEventListener('click', function () {
//    const governorate = document.getElementById('governorate-ad').value;
//    const job = document.getElementById('job-ad').value;
//    const salary = document.getElementById('salary').value;

//    // Show loading indicator
//    const adsContainer = document.querySelector('.ads-container');
//    adsContainer.innerHTML = '<div class="loading">Loading...</div>';

//    fetch(`/Home/FilterAdvertisements?governorate=${encodeURIComponent(governorate)}&job=${encodeURIComponent(job)}&salary=${encodeURIComponent(salary)}`)
//        .then(response => {
//            if (!response.ok) {
//                throw new Error('Network response was not ok');
//            }
//            return response.text();
//        })
//        .then(html => {
//            adsContainer.innerHTML = html;
//        })
//        .catch(error => {
//            console.error('Error filtering advertisements:', error);
//            adsContainer.innerHTML = '<div class="error">Error loading results. Please try again.</div>';
//        });
//});
//document.querySelector('.aa').addEventListener('click', function () {
//    const governorate = document.getElementById('governorate-ad').value;
//    const job = document.getElementById('job-ad').value;
//    const salary = document.getElementById('salary').value;

//    // Show loading indicator
//    const adsContainer = document.querySelector('.ads-container');
//    adsContainer.innerHTML = '<div class="loading">Loading...</div>';

//    fetch(`/Home/FilterAdvertisements?governorate=${encodeURIComponent(governorate)}&job=${encodeURIComponent(job)}&salary=${encodeURIComponent(salary)}`)
//        .then(response => {
//            if (!response.ok) {
//                throw new Error('Network response was not ok');
//            }
//            return response.text();
//        })
//        .then(html => {
//            adsContainer.innerHTML = html;
//        })
//        .catch(error => {
//            console.error('Error filtering advertisements:', error);
//            adsContainer.innerHTML = '<div class="error">Error loading results. Please try again.</div>';
//        });
//});

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