var swiper = new Swiper(".productSwiper", {
    slidesPerView: 1,
    spaceBetween: 20,
    loop: true,
    keyboard: {
        enabled: true,
        onlyInViewport: true
    },
    pagination: {
        el: ".swiper-pagination",
        clickable: true,
    },
    navigation: {
        nextEl: ".swiper-button-next",
        prevEl: ".swiper-button-prev",
    },
    breakpoints: {
        768: { slidesPerView: 2 },
        992: { slidesPerView: 3 },
    },
});


// Apply 3D Tilt
VanillaTilt.init(document.querySelectorAll(".product-card"), {
    max: 15,
    speed: 400,
    glare: true,
    "max-glare": 0.3,
});


//===Cart Added Items (Frontend to Backend) ======
function addToWishlist(product) {
    console.log(product);
    fetch('/Cart/AddToCart', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(product)
    })
        .then(response => {
            if (response.ok) {
                console.log("Added to wishlist!");
            } else {
                console.error("Failed to add to wishlist");
            }
        });
}