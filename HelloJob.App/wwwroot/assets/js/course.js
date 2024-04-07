const likeIcons = document.querySelectorAll('#like');
const unlikeIcons = document.querySelectorAll('#unlike');

likeIcons.forEach(likeIcon => {
    likeIcon.addEventListener('click', function(e) {
        e.preventDefault();
        this.style.display = 'none';
        unlikeIcons[Array.from(likeIcons).indexOf(this)].style.display = 'inline-block';
    });
});

unlikeIcons.forEach(unlikeIcon => {
    unlikeIcon.addEventListener('click', function(e) {
        e.preventDefault();
        this.style.display = 'none';
        likeIcons[Array.from(unlikeIcons).indexOf(this)].style.display = 'inline-block';
    });
});


window.onload = function () {
  slideOne();
  slideTwo();
};

let sliderOne = document.getElementById("slider-1");
let sliderTwo = document.getElementById("slider-2");
let displayValOne = document.getElementById("range1");
let displayValTwo = document.getElementById("range2");
let minGap = 0;
let sliderTrack = document.querySelector(".slider-track");
let sliderMaxValue = document.getElementById("slider-1").max;

function slideOne() {
  if (parseInt(sliderTwo.value) - parseInt(sliderOne.value) <= minGap) {
    sliderOne.value = parseInt(sliderTwo.value) - minGap;
  }
  displayValOne.textContent = sliderOne.value;
  fillColor();
}
function slideTwo() {
  if (parseInt(sliderTwo.value) - parseInt(sliderOne.value) <= minGap) {
    sliderTwo.value = parseInt(sliderOne.value) + minGap;
  }
  
  displayValTwo.textContent = sliderTwo.value;
  fillColor();
}
function fillColor() {
  percent1 = (sliderOne.value / sliderMaxValue) * 100;
  percent2 = (sliderTwo.value / sliderMaxValue) * 100;
  sliderTrack.style.background = `linear-gradient(to right, #dadae5 ${percent1}% , #3264fe ${percent1}% , #3264fe ${percent2}%, #dadae5 ${percent2}%)`;
}


// ------------------------



const filterClearButton = document.getElementById('clear_filters2');
const filterClearButton2 = document.getElementById('clear_filters');
const range1 = document.getElementById('range1');
  const range2 = document.getElementById('range2');
  const slider1 = document.getElementById('slider-1');
  const slider2 = document.getElementById('slider-2');

filterClearButton.addEventListener('click', function(e) {
  e.preventDefault()
    document.getElementById('sort').selectedIndex = 0;
    var checkboxes = document.querySelectorAll('.resumes__filters__checkbox input[type="checkbox"]');
    checkboxes.forEach(function(checkbox) {
        checkbox.checked = false;
    });
    const radioButtons = document.querySelectorAll('input[type="radio"][name="sort"]');
    radioButtons.forEach(function(radioButton,index) {
        radioButton.checked = false;
        if (index === 0) {
          radioButton.checked = true;
      }
    });

    range1.textContent = "0";
    range2.textContent = "24";
    slider1.value = "0";
    slider2.value = "24";
    fillColor()
    
});
filterClearButton2.addEventListener('click', function(e) {
  e.preventDefault()
    document.getElementById('sort').selectedIndex = 0;
    var checkboxes = document.querySelectorAll('.resumes__filters__checkbox input[type="checkbox"]');
    checkboxes.forEach(function(checkbox) {
        checkbox.checked = false;
    });

    
    range1.textContent = "0";
    range2.textContent = "24";
    slider1.value = "0";
    slider2.value = "24";

    fillColor()
});


var filterbtn=document.querySelector('.btn-filters')
var closeButton=document.querySelector('.resumes__filters__close')
var resumesFilter=document.querySelector('#resume_filters')
var sortbtn=document.querySelector('.btn-sort')
var resumesSort=document.querySelector('.resumes__sort')
sortbtn.addEventListener('click',function(e){
  e.preventDefault()
  resumesSort.classList.toggle('resume-show')
  })

filterbtn.addEventListener('click',function(e){
e.preventDefault()
resumesFilter.style.opacity='1'
resumesFilter.style.visibility='visible'
})


closeButton.addEventListener('click',function(e){
e.preventDefault()
resumesFilter.style.opacity='0'
resumesFilter.style.visibility='hidden'
})

