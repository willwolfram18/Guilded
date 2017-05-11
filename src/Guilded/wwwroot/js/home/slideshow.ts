$(document).ready(() => {
    const mySwiper = new Swiper(".swiper-container",
        {
            pagination: ".swiper-pagination",
            paginationClickable: true,
            nextButton: ".swiper-button-next",
            prevButton: ".swiper-button-prev",
            effect: "fade",
            autoplayDisableOnInteraction: false,
            centeredSlides: true,
            loop: true,
            autoplay: 4000
        });
});