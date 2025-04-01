// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

document.addEventListener('DOMContentLoaded', function() {
    // Product Image Preview
    const imageInput = document.querySelector('input[type="file"]');
    if (imageInput) {
        imageInput.addEventListener('change', function(e) {
            if (e.target.files && e.target.files[0]) {
                const reader = new FileReader();
                reader.onload = function(e) {
                    const preview = document.createElement('img');
                    preview.src = e.target.result;
                    preview.style.maxWidth = '200px';
                    preview.style.marginTop = '10px';
                    const container = imageInput.parentElement;
                    const existingPreview = container.querySelector('img');
                    if (existingPreview) {
                        container.removeChild(existingPreview);
                    }
                    container.appendChild(preview);
                }
                reader.readAsDataURL(e.target.files[0]);
            }
        });
    }

    // Price Formatting
    const priceInputs = document.querySelectorAll('input[name$="Price"], input[name$="Price2"]');
    priceInputs.forEach(input => {
        input.addEventListener('input', function(e) {
            let value = e.target.value.replace(/\D/g, '');
            if (value) {
                value = parseInt(value).toLocaleString('vi-VN');
                e.target.value = value;
            }
        });
    });

    // Search Functionality
    const searchInput = document.querySelector('.search-box input');
    const searchButton = document.querySelector('.search-box button');
    
    if (searchInput && searchButton) {
        const performSearch = () => {
            const query = searchInput.value.trim();
            if (query) {
                window.location.href = `/Product/Search?q=${encodeURIComponent(query)}`;
            }
        };

        searchButton.addEventListener('click', performSearch);
        searchInput.addEventListener('keypress', (e) => {
            if (e.key === 'Enter') {
                performSearch();
            }
        });
    }

    // Sticky Header
    const header = document.querySelector('header');
    let lastScroll = 0;

    window.addEventListener('scroll', () => {
        const currentScroll = window.pageYOffset;
        if (currentScroll <= 0) {
            header.classList.remove('scroll-up');
            return;
        }

        if (currentScroll > lastScroll && !header.classList.contains('scroll-down')) {
            header.classList.remove('scroll-up');
            header.classList.add('scroll-down');
        } else if (currentScroll < lastScroll && header.classList.contains('scroll-down')) {
            header.classList.remove('scroll-down');
            header.classList.add('scroll-up');
        }
        lastScroll = currentScroll;
    });

    // Add to Cart Animation
    document.querySelectorAll('.add-to-cart').forEach(button => {
        button.addEventListener('click', function (e) {
            e.preventDefault();
            const cart = document.querySelector('.cart-btn');
            const productCard = this.closest('.card');
            const productImage = productCard.querySelector('img');

            const flyingImage = productImage.cloneNode();
            flyingImage.style.position = 'fixed';
            flyingImage.style.height = '100px';
            flyingImage.style.width = '100px';
            flyingImage.style.objectFit = 'cover';
            flyingImage.style.zIndex = '1000';
            document.body.appendChild(flyingImage);

            const start = productImage.getBoundingClientRect();
            const end = cart.getBoundingClientRect();

            flyingImage.style.top = start.top + 'px';
            flyingImage.style.left = start.left + 'px';

            flyingImage.style.transition = 'all 1s ease-in-out';
            flyingImage.style.transform = 'scale(0.1)';
            flyingImage.style.top = end.top + 'px';
            flyingImage.style.left = end.left + 'px';

            setTimeout(() => {
                flyingImage.remove();
                const badge = cart.querySelector('.badge');
                const currentCount = parseInt(badge.textContent);
                badge.textContent = currentCount + 1;
                badge.style.transform = 'scale(1.5)';
                setTimeout(() => badge.style.transform = 'scale(1)', 200);
            }, 1000);
        });
    });
});

// Scroll to top button
$(window).scroll(function() {
    if ($(this).scrollTop() > 100) {
        $('#scroll-to-top').fadeIn();
    } else {
        $('#scroll-to-top').fadeOut();
    }
});

$('#scroll-to-top').click(function() {
    $('html, body').animate({scrollTop : 0}, 800);
    return false;
});

// Navbar scroll effect
$(window).scroll(function() {
    if ($(this).scrollTop() > 50) {
        $('.navbar').addClass('navbar-scrolled');
    } else {
        $('.navbar').removeClass('navbar-scrolled');
    }
});
