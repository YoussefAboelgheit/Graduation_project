function toggleCard() {
    const flipCard = document.getElementById("flip-card");
    const toggleText = document.getElementById("toggle-text");
  
    if (flipCard.classList.contains("flip")) {
      flipCard.classList.remove("flip");
      toggleText.innerText = "Applicant";
    } else {
      flipCard.classList.add("flip");
      toggleText.innerText = "Company";
    }
}

document.addEventListener('DOMContentLoaded', function () {

    // File validation configurations
    const fileValidationRules = {
        'CVFile': {
            extensions: ['.pdf'],
            message: 'CV must be in .pdf format.',
            maxSize: 5 * 1024 * 1024 // 5MB
        },
        'Logo': {
            extensions: ['.jpg', '.jpeg', '.png'],
            message: 'Logo must have a valid file extension: .jpg, .jpeg, or .png',
            maxSize: 2 * 1024 * 1024 // 2MB
        },
        'TaxCard': {
            extensions: ['.pdf'],
            message: 'Tax card must be a .pdf extension',
            maxSize: 10 * 1024 * 1024 // 10MB
        },
        'CommercialRegister': {
            extensions: ['.pdf'],
            message: 'Commercial register must be a .pdf extension',
            maxSize: 10 * 1024 * 1024 // 10MB
        },
        'ProfileImage': {
            extensions: ['.jpg', '.jpeg', '.png'],
            message: 'Profile Image must have a valid file extension: .jpg, .jpeg, or .png',
            maxSize: 2 * 1024 * 1024 // 2MB
        },
    };

    // Function to validate file
    function validateFile(file, rules) {
        if (!file) return { isValid: true, message: '' };

        const fileName = file.name.toLowerCase();
        const fileSize = file.size;
        const fileExtension = '.' + fileName.split('.').pop();

        // Check file extension
        if (!rules.extensions.includes(fileExtension)) {
            return {
                isValid: false,
                message: rules.message
            };
        }

        // Check file size
        if (fileSize > rules.maxSize) {
            const maxSizeMB = (rules.maxSize / (1024 * 1024)).toFixed(1);
            return {
                isValid: false,
                message: `File size must be less than ${maxSizeMB}MB`
            };
        }

        return { isValid: true, message: '' };
    }

    // Function to show/hide error message
    function showError(inputElement, message) {
        // Remove existing error message
        const existingError = inputElement.parentNode.querySelector('.client-validation-error');
        if (existingError) {
            existingError.remove();
        }

        if (message) {
            // Create and show error message
            const errorSpan = document.createElement('span');
            errorSpan.className = 'text-danger client-validation-error';
            errorSpan.textContent = message;
            errorSpan.style.display = 'block';
            errorSpan.style.fontSize = '0.875em';
            errorSpan.style.marginTop = '5px';

            inputElement.parentNode.insertBefore(errorSpan, inputElement.nextSibling);
            inputElement.style.borderColor = '#dc3545'; // Red border
        } else {
            // Remove error styling
            inputElement.style.borderColor = '';
        }
    }

    // Function to show allowed extensions tooltip
    function showAllowedExtensions(inputElement, extensions) {
        const tooltip = document.createElement('div');
        tooltip.className = 'file-extensions-tooltip';
        tooltip.innerHTML = `
            <small style="color: #6c757d; font-size: 0.8em;">
                Allowed: ${extensions.join(', ')}
            </small>
        `;

        // Remove existing tooltip
        const existingTooltip = inputElement.parentNode.querySelector('.file-extensions-tooltip');
        if (existingTooltip) {
            existingTooltip.remove();
        }

        inputElement.parentNode.insertBefore(tooltip, inputElement.nextSibling);
    }

    // Add event listeners to all file inputs
    Object.keys(fileValidationRules).forEach(fieldName => {
        const fileInput = document.querySelector(`input[name="${fieldName}"]`);
        if (fileInput) {
            const rules = fileValidationRules[fieldName];

            // Show allowed extensions on focus
            fileInput.addEventListener('focus', function () {
                showAllowedExtensions(this, rules.extensions);
            });

            // Validate on file selection
            fileInput.addEventListener('change', function () {
                const file = this.files[0];
                const validation = validateFile(file, rules);

                if (!validation.isValid) {
                    showError(this, validation.message);
                    this.value = ''; // Clear the invalid file
                } else {
                    showError(this, ''); // Clear any existing error
                }
            });

            // Remove tooltip on blur (optional)
            fileInput.addEventListener('blur', function () {
                setTimeout(() => {
                    const tooltip = this.parentNode.querySelector('.file-extensions-tooltip');
                    if (tooltip) {
                        tooltip.remove();
                    }
                }, 200);
            });
        }
    });

    // Form submission validation
    const forms = document.querySelectorAll('form');
    forms.forEach(form => {
        form.addEventListener('submit', function (e) {
            let isFormValid = true;

            // Validate all file inputs in this form
            const fileInputs = form.querySelectorAll('input[type="file"]');
            fileInputs.forEach(input => {
                const fieldName = input.name;
                const rules = fileValidationRules[fieldName];

                if (rules) {
                    const file = input.files[0];
                    const validation = validateFile(file, rules);

                    if (!validation.isValid) {
                        showError(input, validation.message);
                        isFormValid = false;
                    }
                }
            });


            // Email validation function
            async function checkEmailExists(email) {
                try {
                    const response = await fetch('/Home/CheckEmailExists', {
                        method: 'POST',
                        headers: {
                            'Content-Type': 'application/json',
                            'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]')?.value || ''
                        },
                        body: JSON.stringify(email)
                    });

                    const result = await response.json();
                    return result.exists;
                } catch (error) {
                    console.error('Error checking email:', error);
                    return false;
                }
            }

            // Function to show email error
            function showEmailError(inputElement, message) {
                // Remove existing email error
                const existingError = inputElement.parentNode.querySelector('.email-validation-error');
                if (existingError) {
                    existingError.remove();
                }

                if (message) {
                    // Create and show error message
                    const errorSpan = document.createElement('span');
                    errorSpan.className = 'text-danger email-validation-error';
                    errorSpan.textContent = message;
                    errorSpan.style.display = 'block';
                    errorSpan.style.fontSize = '0.875em';
                    errorSpan.style.marginTop = '5px';

                    inputElement.parentNode.insertBefore(errorSpan, inputElement.nextSibling);
                    inputElement.style.borderColor = '#dc3545'; // Red border
                } else {
                    // Remove error styling
                    inputElement.style.borderColor = '';
                }
            }

            // Add email validation to all email inputs
            const emailInputs = document.querySelectorAll('input[type="email"]');
            emailInputs.forEach(emailInput => {
                let timeoutId;

                emailInput.addEventListener('blur', async function () {
                    const email = this.value.trim();

                    // Clear any existing timeout
                    if (timeoutId) {
                        clearTimeout(timeoutId);
                    }

                    // Only check if email is not empty and has basic email format
                    if (email && email.includes('@')) {
                        // Add a small delay to avoid too many requests
                        timeoutId = setTimeout(async () => {
                            // Show loading state
                            showEmailError(this, 'Checking email availability...');
                            this.style.borderColor = '#ffc107'; // Yellow border for loading

                            try {
                                const exists = await checkEmailExists(email);

                                if (exists) {
                                    showEmailError(this, 'This email is already registered. Please use a different email.');
                                } else {
                                    showEmailError(this, ''); // Clear error
                                    this.style.borderColor = '#28a745'; // Green border for valid
                                }
                            } catch (error) {
                                showEmailError(this, 'Error checking email. Please try again.');
                            }
                        }, 500); // 500ms delay
                    } else {
                        showEmailError(this, ''); // Clear error if email is empty or invalid format
                    }
                });

                // Clear validation on input (when user is typing)
                emailInput.addEventListener('input', function () {
                    if (timeoutId) {
                        clearTimeout(timeoutId);
                    }
                    // Remove email validation error when user starts typing
                    const existingError = this.parentNode.querySelector('.email-validation-error');
                    if (existingError) {
                        existingError.remove();
                        this.style.borderColor = '';
                    }
                });
            });

            // Update form submission validation to include email check
            const forms = document.querySelectorAll('form');
            forms.forEach(form => {
                form.addEventListener('submit', async function (e) {
                    let isFormValid = true;

                    // Check for email validation errors
                    const emailErrors = form.querySelectorAll('.email-validation-error');
                    emailErrors.forEach(error => {
                        if (error.textContent.includes('already registered') || error.textContent.includes('Error checking')) {
                            isFormValid = false;
                        }
                    });

                    // Validate all file inputs in this form (existing code)
                    const fileInputs = form.querySelectorAll('input[type="file"]');
                    fileInputs.forEach(input => {
                        const fieldName = input.name;
                        const rules = fileValidationRules[fieldName];

                        if (rules) {
                            const file = input.files[0];
                            const validation = validateFile(file, rules);

                            if (!validation.isValid) {
                                showError(input, validation.message);
                                isFormValid = false;
                            }
                        }
                    });

                    if (!isFormValid) {
                        e.preventDefault(); // Prevent form submission

                        // Scroll to first error
                        const firstError = form.querySelector('.client-validation-error, .email-validation-error');
                        if (firstError) {
                            firstError.scrollIntoView({ behavior: 'smooth', block: 'center' });
                        }
                    }
                });
            });


            if (!isFormValid) {
                e.preventDefault(); // Prevent form submission

                // Scroll to first error
                const firstError = form.querySelector('.client-validation-error');
                if (firstError) {
                    firstError.scrollIntoView({ behavior: 'smooth', block: 'center' });
                }
            }
        });
    });
});
