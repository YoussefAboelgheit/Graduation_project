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










































// *********************advs***************************
document.addEventListener('DOMContentLoaded', function() {
    const detailButtons = document.querySelectorAll('.details-btn');
  
    detailButtons.forEach(button => {
      button.addEventListener('click', function() {
        alert('More details will be displayed here!');
      });
    });
  });
  