  window.onload = function () {
      slideOne();
      slideTwo();
      slide_3();
      slide_4();
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
  // ----------------------------------
    
    let slider_3 = document.getElementById("slider-3");
    let slider_4 = document.getElementById("slider-4");
    let displayVal_3 = document.getElementById("range3");
    let displayVal_4 = document.getElementById("range4");
    
    let sliderTrack_internship = document.querySelector(".slider-track_internship");
    let sliderMaxValue_internship = slider_3.max;
    
    function slide_3() {
      if (parseInt(slider_4.value) - parseInt(slider_3.value) <= minGap) {
        slider_3.value = parseInt(slider_4.value) - minGap;
      }
      displayVal_3.textContent = slider_3.value;
      fillColor_2();
    }
    
    function slide_4() {
      if (parseInt(slider_4.value) - parseInt(slider_3.value) <= minGap) {
          slider_4.value = parseInt(slider_3.value) + minGap;
        }
        displayVal_4.textContent = slider_4.value;
        fillColor_2();
      }
      
      function fillColor_2() {
        const percent1 = (slider_3.value / sliderMaxValue_internship) * 100;
        const percent2 = (slider_4.value / sliderMaxValue_internship) * 100;
        sliderTrack_internship.style.background = `linear-gradient(to right, #dadae5 ${percent1}%, #3264fe ${percent1}%, #3264fe ${percent2}%, #dadae5 ${percent2}%)`;
        console.log('salam');
      }
      
  
$(document).ready(function () {
    $('body').on('click', '.course_like #like', function (e) {
        e.preventDefault();
        console.log("new version")
        var likeIcon = $(this);
        var itemId = $(this).closest('.course_like').data('id');
        var itemType = "resume";

        $.ajax({
            url: '/Resume/AddWishlist',
            type: 'POST',
            dataType: 'json',
            data: {
                itemid: itemId,
                itemtype: itemType
            },
            success: function (response) {
                console.log("Wishlist'a elave olundu");
                if (likeIcon.hasClass('fa-regular')) {
                    console.log("dolu ureke deyisdi")
                    likeIcon.removeClass('fa-regular').addClass('fa-solid');
                } else {
                    console.log("dolu ureke deyisdi")
                    likeIcon.removeClass('fa-solid').addClass('fa-regular');
                }
            },
            error: function (xhr, status, error) {
                console.error('There has been a problem with your AJAX request:', error);
            }
        });
    });
});


  const filterClearButton = document.getElementById('clear');
  const filterClearButton2 = document.getElementById('clear_filters');
  const range1 = document.getElementById('range1');
  const range2 = document.getElementById('range2');
  const slider1 = document.getElementById('slider-1');
  const slider2 = document.getElementById('slider-2');
  const range3 = document.getElementById('range3');
  const range4 = document.getElementById('range4');
  const slider3 = document.getElementById('slider-3');
  const slider4 = document.getElementById('slider-4');
  filterClearButton.addEventListener('click', function(e) {
    e.preventDefault()
    console.log("Elcan");
      document.getElementById('sort').selectedIndex = 0;
      var checkboxes = document.querySelectorAll('.resumes__filters__checkbox input[type="checkbox"]');
      checkboxes.forEach(function(checkbox) {
          checkbox.checked = false;
      });
      range1.textContent = "0";
      range2.textContent = "6";
      slider1.value = "0";
      slider2.value = "6";
      range3.textContent = "100";
      range4.textContent = "10000";
      slider3.value = "100";
      slider4.value = "10000";
     const percent3 = (slider1.value / sliderMaxValue) * 100;
     const  percent4 = (slider2.value / sliderMaxValue) * 100;
      sliderTrack.style.background = `linear-gradient(to right, #dadae5 ${percent3}% , #3264fe ${percent4}% , #3264fe ${percent2}%, #dadae5 ${percent2}%)`;
  
      const percent_1 = (slider3.value / sliderMaxValue_internship) * 100;
      const percent_2 = (slider4.value / sliderMaxValue_internship) * 100;
      sliderTrack_internship.style.background = `linear-gradient(to right, #dadae5 ${percent_1}%, #3264fe ${percent1}%, #3264fe ${percent_2}%, #dadae5 ${percent2}%)`;
  
  
  });
  filterClearButton2.addEventListener('click', function(e) {
    e.preventDefault()
      document.getElementById('sort').selectedIndex = 0;
      var checkboxes = document.querySelectorAll('.resumes__filters__checkbox input[type="checkbox"]');
      checkboxes.forEach(function(checkbox) {
          checkbox.checked = false;
      });
      range1.textContent = "0";
      range2.textContent = "6";
      slider1.value = "0";
      slider2.value = "6";
      range3.textContent = "100";
      range4.textContent = "10000";
      slider3.value = "100";
      slider4.value = "10000";
  
  const percent3 = (slider1.value / sliderMaxValue) * 100;
     const  percent4 = (slider2.value / sliderMaxValue) * 100;
      sliderTrack.style.background = `linear-gradient(to right, #dadae5 ${percent3}% , #3264fe ${percent4}% , #3264fe ${percent2}%, #dadae5 ${percent2}%)`;
  
      const percent_1 = (slider3.value / sliderMaxValue_internship) * 100;
      const percent_2 = (slider4.value / sliderMaxValue_internship) * 100;
      sliderTrack_internship.style.background = `linear-gradient(to right, #dadae5 ${percent_1}%, #3264fe ${percent1}%, #3264fe ${percent_2}%, #dadae5 ${percent2}%)`;
      const radioButtons = document.querySelectorAll('input[type="radio"][name="sort"]');
  radioButtons.forEach(function(radioButton,index) {
      radioButton.checked = false;
      if (index === 0) {
        radioButton.checked = true;
    }
  });
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
