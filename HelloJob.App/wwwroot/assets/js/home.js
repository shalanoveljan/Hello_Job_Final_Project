const filterClearButton = document.getElementById('FilterTemizle');
filterClearButton.addEventListener('click', function() {
    var selectInputs = document.querySelectorAll('.form-select');
    selectInputs.forEach(function(selectInput) {
        selectInput.selectedIndex = 0;
    });

    var checkboxes = document.querySelectorAll('.checkbox');
    checkboxes.forEach(function(checkbox) {
        checkbox.checked = false;
    });
});

const MultiSearch=document.getElementById('multisearch');
const searchbar=document.querySelector('.nsb_searchbar_filter');
const topsearchbar=document.querySelector('.nsb_searchbar_top')
const search_closebutton=document.querySelector('.nsp_close');

MultiSearch.addEventListener('click',function(){
searchbar.style.display="block";
topsearchbar.style.display="none";

})

search_closebutton.addEventListener('click',function(){
    searchbar.style.display="none";
topsearchbar.style.display="flex";
})

$('.new-categories-slider').slick({
    dots: false,
    infinite: true,
    speed: 300,
    slidesToShow: 5,
    slidesToScroll: 1,
    
    responsive: [
      {
        breakpoint: 1024,
        settings: {
          slidesToShow: 3,
          slidesToScroll: 3,
          infinite: true,
          dots: true
        }
      },
      {
        breakpoint: 600,
        settings: {
          slidesToShow: 2,
          slidesToScroll: 2
        }
      },
      {
        breakpoint: 560,
        settings: {
          slidesToShow: 3,
          slidesToScroll: 1,
          arrows:false


        }
      }
      // You can unslick at a given breakpoint now by adding:
      // settings: "unslick"
      // instead of a settings object
    ]
  });


    $('.new-companies').slick({
      dots: false,
      infinite: true,
      speed: 300,
      slidesToShow: 5,
      slidesToScroll: 1,
      
      responsive: [
        {
          breakpoint: 1024,
          settings: {
            slidesToShow: 3,
            slidesToScroll: 3,
            infinite: true,
            dots: true
          }
        },
        {
          breakpoint: 600,
          settings: {
            slidesToShow: 2,
            slidesToScroll: 2
          }
        },
        {
          breakpoint: 560,
          settings: {
            slidesToShow: 3,
            slidesToScroll: 1,
            arrows:false
  
  
          }
        }
        // You can unslick at a given breakpoint now by adding:
        // settings: "unslick"
        // instead of a settings object
      ]
    });
  

 //const likeIcons = document.querySelectorAll('#like');
 //const unlikeIcons = document.querySelectorAll('#unlike');
 
 //likeIcons.forEach(likeIcon => {
 //    likeIcon.addEventListener('click', function(e) {
 //        e.preventDefault();
 //        this.style.display = 'none';
 //        unlikeIcons[Array.from(likeIcons).indexOf(this)].style.display = 'inline-block';
 //    });
 //});
 
 //unlikeIcons.forEach(unlikeIcon => {
 //    unlikeIcon.addEventListener('click', function(e) {
 //        e.preventDefault();
 //        this.style.display = 'none';
 //        likeIcons[Array.from(unlikeIcons).indexOf(this)].style.display = 'inline-block';
 //    });
 //});


$(document).ready(function () {
    $('.course_like').on('click', '#like', function (e) {
        e.preventDefault();
        var likeIcon = $(this);

        var itemId = $(this).closest('.course_like').data('id');
        var itemType = "vacancy";

        $.ajax({
            url: '/Home/AddWishlist',
            type: 'POST',
            dataType: 'json',
            data: {
                itemid: itemId,
                itemtype: itemType
            },
            success: function (response) {
                console.log("Wishlist'a elave olundu");
                if (likeIcon.hasClass('fa-regular')) {
                    likeIcon.removeClass('fa-regular').addClass('fa-solid');
                } else {
                    likeIcon.removeClass('fa-solid').addClass('fa-regular');
                }
            },
            error: function (xhr, status, error) {
                console.error('There has been a problem with your AJAX request:', error);
            }
        });
    });
});
 

