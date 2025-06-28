document.addEventListener('DOMContentLoaded', function () {
    const form1 = document.getElementById('form-1');
    const form2 = document.getElementById('form-2');
    const nextBtn = document.getElementById('next-btn');
    const prevBtn = document.getElementById('prev-btn');
    const addQuestionBtn = document.getElementById('add-question-btn');
    const questionsContainer = document.querySelector('.questions');
    const doneBtn = document.getElementById('done-btn');

    let questionCount = 0;

    // Navigation between forms
    nextBtn.addEventListener('click', function () {
        // Validate form 1 before proceeding
        const form1Inputs = form1.querySelectorAll('input[required], select[required], textarea[required]');
        let isValid = true;

        form1Inputs.forEach(input => {
            if (!input.value.trim()) {
                isValid = false;
                input.style.borderColor = 'red';
            } else {
                input.style.borderColor = '';
            }
        });

        if (isValid) {
            form1.style.display = 'none';
            form2.style.display = 'block';
            nextBtn.style.display = 'none';
            prevBtn.style.display = 'inline-block';
            addQuestionBtn.style.display = 'block';
        } else {
            alert('Please fill in all required fields.');
        }
    });

    prevBtn.addEventListener('click', function () {
        form2.style.display = 'none';
        form1.style.display = 'block';
        nextBtn.style.display = 'inline-block';
        prevBtn.style.display = 'none';
        addQuestionBtn.style.display = 'none';
    });

    // Add question functionality
    addQuestionBtn.addEventListener('click', function () {
        questionCount++;
        const questionDiv = document.createElement('div');
        questionDiv.className = 'question-item';

        // Create the question input as a hidden input in form-1 and visible input in form-2
        const hiddenInput = document.createElement('input');
        hiddenInput.type = 'hidden';
        hiddenInput.name = `CustomQuestions[${questionCount - 1}]`;
        hiddenInput.id = `hidden_question_${questionCount}`;
        form1.appendChild(hiddenInput);

        questionDiv.innerHTML = `
            <div class="form-group">
                <label for="question_${questionCount}">Question ${questionCount}:</label>
                <input type="text" 
                       id="question_${questionCount}" 
                       placeholder="Enter your question"
                       class="question-input"
                       data-hidden-id="hidden_question_${questionCount}"
                       oninput="updateHiddenInput(this)">
                <button type="button" class="remove-question-btn" onclick="removeQuestion(this)">Remove</button>
            </div>
        `;
        questionsContainer.appendChild(questionDiv);
    });

    // Form submission handler
    doneBtn.addEventListener('click', function (e) {
        e.preventDefault();

        // Final update of all hidden inputs before submission
        const questionInputs = document.querySelectorAll('.question-input');
        questionInputs.forEach((input) => {
            updateHiddenInput(input);
        });

        // Debug: Log what we're submitting
        const hiddenInputs = document.querySelectorAll('input[name^="CustomQuestions"]');
        console.log('Submitting questions:');
        hiddenInputs.forEach((input, index) => {
            console.log(`Question ${index}: ${input.value}`);
        });

        // Submit the form
        document.querySelector('form').submit();
    });
});

// Function to update hidden input when question text changes
function updateHiddenInput(visibleInput) {
    const hiddenId = visibleInput.getAttribute('data-hidden-id');
    const hiddenInput = document.getElementById(hiddenId);
    if (hiddenInput) {
        hiddenInput.value = visibleInput.value;
    }
}

// Remove question function
function removeQuestion(button) {
    const questionItem = button.closest('.question-item');
    const questionInput = questionItem.querySelector('.question-input');
    const hiddenId = questionInput.getAttribute('data-hidden-id');

    // Remove the corresponding hidden input
    const hiddenInput = document.getElementById(hiddenId);
    if (hiddenInput) {
        hiddenInput.remove();
    }

    // Remove the visible question
    questionItem.remove();

    // Re-index remaining questions
    reindexQuestions();
}

function reindexQuestions() {
    const questionInputs = document.querySelectorAll('.question-input');
    const form1 = document.getElementById('form-1');

    // Remove all existing hidden inputs for questions
    const existingHiddenInputs = form1.querySelectorAll('input[name^="CustomQuestions"]');
    existingHiddenInputs.forEach(input => input.remove());

    // Recreate hidden inputs with correct indexing
    questionInputs.forEach((input, index) => {
        // Update visible input attributes
        const label = input.closest('.form-group').querySelector('label');
        label.textContent = `Question ${index + 1}:`;
        label.setAttribute('for', `question_${index + 1}`);
        input.id = `question_${index + 1}`;

        // Create new hidden input with correct index
        const hiddenInput = document.createElement('input');
        hiddenInput.type = 'hidden';
        hiddenInput.name = `CustomQuestions[${index}]`;
        hiddenInput.id = `hidden_question_${index + 1}`;
        hiddenInput.value = input.value;
        form1.appendChild(hiddenInput);

        // Update data attribute
        input.setAttribute('data-hidden-id', `hidden_question_${index + 1}`);
    });
}