// **************************التقسيمه*********************
// الكود أدناه اختياري ويمكنك تعديله لاحقاً حسب متطلبات التنقل أو الوظائف الأخرى

document.addEventListener('DOMContentLoaded', () => {
    // Job item click logging
    const jobItems = document.querySelectorAll('.job-item');
    jobItems.forEach(item => {
        item.addEventListener('click', (e) => {
            console.log(`Clicked on: ${item.querySelector('span').textContent}`);
        });
    });

    // Applicant filter apply
    const applicantBtn = document.querySelector('.ie');
    if (applicantBtn) {
        applicantBtn.addEventListener('click', function () {
            const governorate = document.getElementById('governorate-app').value;
            const experience = document.getElementById('experience').value;
            const job = document.getElementById('job').value;

            fetch(`/Home/FilterApplicants?governorate=${encodeURIComponent(governorate)}&experience=${encodeURIComponent(experience)}&job=${encodeURIComponent(job)}`)
                .then(response => response.text())
                .then(html => {
                    document.getElementById('Applicants').innerHTML = html;
                });
        });
    }

    // Advertisement filter apply
    const advBtn = document.querySelector('.aa');
    if (advBtn) {
        advBtn.addEventListener('click', function () {
            const governorate = document.getElementById('governorate-ad').value;
            const job = document.getElementById('job-ad').value;
            const workType = document.getElementById('work-type').value;
            const salary = document.getElementById('salary').value;

            const adsContainer = document.querySelector('.ads-container');

            fetch(`/Home/FilterAdvertisements?governorate=${encodeURIComponent(governorate)}&job=${encodeURIComponent(job)}&workType=${encodeURIComponent(workType)}&salary=${encodeURIComponent(salary)}`)
                .then(response => {
                    if (!response.ok) throw new Error('Network response was not ok');
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
    }


    const input = document.getElementById('searchInput');
    const suggestionsBox = document.getElementById('suggestions');
    const box = document.querySelector('.box');

    if (input && suggestionsBox && box) {
        input.addEventListener('input', () => {
            const query = input.value.trim();
            if (query.length < 1) {
                suggestionsBox.innerHTML = '';
                suggestionsBox.style.display = 'none';
                return;
            }

            fetch(`/Home/Suggestions?q=${encodeURIComponent(query)}`)
                .then(response => response.json())
                .then(data => {
                    suggestionsBox.innerHTML = '';
                    suggestionsBox.style.display = 'block';

                    const getIconHtml = (source) => {
                        switch (source) {
                            case "applicant": return '<i class="fas fa-user" title="Applicant"></i>';
                            case "ad": return '<i class="fas fa-bullhorn" title="Job Advertisement"></i>';
                            case "company": return '<i class="fas fa-building" title="Company"></i>';
                            default: return '<i class="fas fa-question-circle"></i>';
                        }
                    };

                    const buildSection = (title, items) => {
                        if (!items || items.length === 0) return '';
                        let html = `<div class="suggestion-header">${title}</div>`;
                        for (const item of items) {
                            const icon = getIconHtml(item.source);
                            html += `
                        <div class="suggestion-item" data-type="${item.type}" data-id="${item.id}" data-source="${item.source}">
                            ${icon}
                            <div style="display: inline-block; margin-left: 10px;">
                                <div><strong>${item.label}</strong></div>
                                ${item.sub ? `<div style="font-size: 0.85em; color: #666;">${item.sub}</div>` : ''}
                            </div>
                        </div>`;
                        }
                        return html;
                    };

                    const html =
                        buildSection('Field of Work', data.fieldOfWork) +
                        buildSection('City', data.city) +
                        buildSection('Username', data.username);

                    suggestionsBox.innerHTML = html;

                    document.querySelectorAll('.suggestion-item').forEach(div => {
                        div.addEventListener('click', () => {
                            const type = div.dataset.type;
                            const id = div.dataset.id;
                            const source = div.dataset.source;

                            if (type === "applicantField" || type === "applicantCity" || (type === "user" && source === "applicant")) {
                                window.location.href = `/ApplicantInfo/Index/${id}`;
                            } else if (type === "adField" || type === "adCity") {
                                window.location.href = `/JobAdvertisement/Details/${id}`;
                            } else if (type === "user" && source === "company") {
                                window.location.href = `/CompanyInfo/Index/${id}`;
                            } else {
                                alert("Unknown item type.");
                            }
                        });
                    });
                })
                .catch(() => {
                    suggestionsBox.innerHTML = '';
                    suggestionsBox.style.display = 'none';
                });
        });

        // ✅ New: Handle Enter key
        input.addEventListener('keydown', (e) => {
            if (e.key === 'Enter') {
                const query = input.value.trim();
                if (query.length > 0) {
                    window.location.href = `/Search/Index?q=${encodeURIComponent(query)}`;
                }
            }
        });

        // 👇 Blur hides suggestion (delay allows click)
        input.addEventListener('blur', () => {
            setTimeout(() => {
                suggestionsBox.style.display = 'none';
                suggestionsBox.innerHTML = '';
            }, 150);
        });

        // 👇 Focus ensures input expands again
        input.addEventListener('focus', () => {
            input.style.width = '400px';
        });

        // 👇 Clicking anywhere else hides suggestions
        document.addEventListener('click', (e) => {
            if (!box.contains(e.target)) {
                input.blur(); // shrink input
                suggestionsBox.innerHTML = '';
                suggestionsBox.style.display = 'none';
            }
        });
    }



});


