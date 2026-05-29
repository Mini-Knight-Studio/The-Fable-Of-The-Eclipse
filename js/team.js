var prevIndex = '';

function ChangeDisplay(memberId)
{
    var member_card = document.getElementById(memberId);
    
    var member_card_display_button = member_card.querySelector('.member-button');
    var member_card_display_button_icon = member_card.querySelector('.icon');
    var member_card_display = member_card.querySelector('.member-display');
    
    var status = member_card_display_button.getAttribute('status');

    if (status === 'off')
    {
        member_card_display_button.setAttribute('status', 'on');
        member_card_display.style.display = 'flex';
        member_card_display_button_icon.setAttribute('icon', 'material-symbols:arrow-drop-up-rounded');
    }
    else if (status === 'on')
    {
        member_card_display_button.setAttribute('status', 'off');
        member_card_display.style.display = 'none';
        member_card_display_button_icon.setAttribute('icon', 'material-symbols:arrow-drop-down-rounded');
    }
}

function HideCard(memberID)
{
    var card = document.getElementById(memberID);
    var profile = card.querySelector('.show-profile');
    var display = card.querySelector('.display');

    display.style.width = 0;
    display.style.height = 0;
    display.style.opacity = 0;
    display.style.visibility = 'hidden';

    profile.style.width = 'fit-content';
    profile.style.height = 'fit-content';
    profile.style.visibility = 'visible';
    profile.style.opacity = 1;

    stopCarouselAutoScroll()
}

function ShowCard(memberID)
{    
    var card = document.getElementById(memberID);
    var profile = card.querySelector('.show-profile');
    var display = card.querySelector('.display');

    profile.style.width = 0;
    profile.style.height = 0;
    profile.style.opacity = 0;
    profile.style.visibility = 'hidden';

    display.style.width = 'fit-content';
    display.style.height = 'fit-content';
    display.style.visibility = 'visible';
    display.style.opacity = 1;

    startCarouselAutoScroll(memberID);
}

function DisplayCard(memberID)
{
    if(prevIndex !== memberID)
    {
        if(prevIndex != '')
            HideCard(prevIndex);
        ShowCard(memberID);
        prevIndex = memberID;
    }
    else
    {
        HideCard(prevIndex);
        prevIndex = '';
    }
}


let autoScrollTimer = null; 
const AUTO_SCROLL_SPEED = 4000;

function updateCarousel(carouselId, targetSlide) {
    const carousel = document.getElementById(carouselId);
    if (!carousel) return;

    const tracks = carousel.querySelectorAll('.carousel-track');
    const totalSlides = tracks.length;

    if (targetSlide > totalSlides) targetSlide = 1;
    if (targetSlide < 1) targetSlide = totalSlides;

    carousel.setAttribute('slide', targetSlide);

    tracks.forEach(track => {
        const trackNum = parseInt(track.getAttribute('slide'));
        // Target the YouTube iframe instead of a native video element
        const iframe = track.querySelector('iframe'); 

        if (trackNum === targetSlide) {
            track.classList.add('active');
            
            // --- LOAD YOUTUBE VIDEO ---
            if (iframe && iframe.dataset.src) {
                iframe.src = iframe.dataset.src; 
            }
        } else {
            track.classList.remove('active');
            
            // --- UNLOAD YOUTUBE VIDEO ---
            if (iframe) {
                iframe.removeAttribute('src'); 
            }
        }
    });
}

// Call this function whenever a member card is opened/shown
function startCarouselAutoScroll(carouselId) {
    // 1. Clear any existing timer from a previously opened card
    clearInterval(autoScrollTimer);

    // 2. Initialize the carousel to its current slide
    const carousel = document.getElementById(carouselId);
    if (!carousel) return;
    const currentSlide = parseInt(carousel.getAttribute('slide')) || 1;
    updateCarousel(carouselId, currentSlide);

    // 3. Start the single loop for this specific carousel
    autoScrollTimer = setInterval(() => {
        const activeCarousel = document.getElementById(carouselId);
        if (activeCarousel) {
            const slide = parseInt(activeCarousel.getAttribute('slide')) || 1;
            updateCarousel(carouselId, slide + 1);
        }
    }, AUTO_SCROLL_SPEED);
}

// Call this function when a card is closed/hidden
function stopCarouselAutoScroll() {
    clearInterval(autoScrollTimer);
}

// Button Controls
function SetPreviousTask(carouselId) {
    const carousel = document.getElementById(carouselId);
    if (!carousel) return;

    const currentSlide = parseInt(carousel.getAttribute('slide')) || 1;
    updateCarousel(carouselId, currentSlide - 1);
    
    // Reset the timer window so it doesn't jump immediately after clicking
    startCarouselAutoScroll(carouselId); 
}

function SetNextTask(carouselId) {
    const carousel = document.getElementById(carouselId);
    if (!carousel) return;

    const currentSlide = parseInt(carousel.getAttribute('slide')) || 1;
    updateCarousel(carouselId, currentSlide + 1);
    
    // Reset the timer window
    startCarouselAutoScroll(carouselId); 
}

// Run this code to automatically attach click listeners to all tracks on the page
document.addEventListener("DOMContentLoaded", () => {
    const allTracks = document.querySelectorAll('.carousel-track');
    
    allTracks.forEach(track => {
        track.addEventListener('click', () => {
            console.log("Track clicked. Auto-scroll stopped.");
            stopCarouselAutoScroll();
        });
    });
});