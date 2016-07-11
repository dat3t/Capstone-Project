var content = [
  { title: 'Andorra' },
  { title: 'United Arab Emirates' },
  { title: 'Afghanistan' },
  { title: 'Antigua' },
  { title: 'Anguilla' },
  { title: 'Albania' },
  { title: 'Armenia' },
  { title: 'Netherlands Antilles' },
  { title: 'Angola' },
  { title: 'Argentina' },
  { title: 'American Samoa' },
  { title: 'Austria' },
  { title: 'Australia' },
  { title: 'Aruba' },
  { title: 'Aland Islands' },
  { title: 'Azerbaijan' },
  { title: 'Bosnia' },
  { title: 'Barbados' },
  { title: 'Bangladesh' },
  { title: 'Belgium' },
  { title: 'Burkina Faso' },
  { title: 'Bulgaria' },
  { title: 'Bahrain' },
  { title: 'Burundi' }
  // etc
];
$(document).ready(function () {
    $(".right.menu.open").on("click", function (e) {
        e.preventDefault();
        $(".ui.vertical.menu.open").toggle();
/*-------------------------------------------------------------------------------------------------------------------------------*/
/*This is main JS file that contains custom style rules used in this template*/
/*-------------------------------------------------------------------------------------------------------------------------------*/
/* Template Name: Site Title*/
/* Version: 1.0 Initial Release*/
/* Build Date: 22-04-2015*/
/* Author: Unbranded*/
/* Website: http://moonart.net.ua/site/ 
/* Copyright: (C) 2015 */
/*-------------------------------------------------------------------------------------------------------------------------------*/

/*--------------------------------------------------------*/
/* TABLE OF CONTENTS: */
/*--------------------------------------------------------*/
/* 01 - VARIABLES */
/* 02 - page calculations */
/* 03 - function on document ready */
/* 04 - function on page load */
/* 05 - function on page resize */
/* 06 - swiper sliders */
/* 07 - buttons, clicks, hovers */
/*-------------------------------------------------------------------------------------------------------------------------------*/

$(function () {
    $("div#myId").dropzone({ url: "/file/post" });

//     
//    $("#imgUpload").click(function () {
//        $("#files").click();
//    });
    $('.delete.icon.image-add').on('click', function () {
        $(this).parent().remove();;
    });

    $('.item.write').click(function () {


        $('.ui.modal').modal('show');

    });

    $('.ui.dropdown')
  .dropdown({
      allowAdditions: true
  })
    ;

    $('.ui.dropdown')
  .dropdown({
      allowAdditions: true
  })
    ;    
   

	"use strict";

	/*================*/
	/* 01 - VARIABLES */
	/*================*/
	var swipers = [], winW, winH, winScr, _isresponsive, smPoint = 768, mdPoint = 992, lgPoint = 1200, addPoint = 1600, _ismobile = navigator.userAgent.match(/Android/i) || navigator.userAgent.match(/webOS/i) || navigator.userAgent.match(/iPhone/i) || navigator.userAgent.match(/iPad/i) || navigator.userAgent.match(/iPod/i);

	/*========================*/
	/* 02 - page calculations */
	/*========================*/
	function pageCalculations(){
		winW = $(window).width();
		winH = $(window).height();
		if($('.menu-button').is(':visible')) _isresponsive = true;
		else _isresponsive = false;
		if(winW <= 992)
			$(".header-menu").css({"max-height":winH - 20 +  "px"});
	}

	/*=================================*/
	/* 03 - function on document ready */
	/*=================================*/
	pageCalculations();

	//center all images inside containers
	$('.center-image').each(function(){
		var bgSrc = $(this).attr('src');
		$(this).parent().addClass('background-block').css({'background-image':'url('+bgSrc+')'});
		$(this).hide();
	});		

	/*============================*/
	/* 04 - function on page load */
	/*============================*/
	$(window).load(function(){
		$(".be-loader").fadeOut("slow");
		initSwiper();
		notification();
		$('.isotope-grid').isotope({
			itemSelector: '.isotope-item ',
			percentPosition: true
		});		
	});

	/*==============================*/
	/* 05 - function on page resize */
	/*==============================*/
	$(window).resize(function(){
			resizeCall();
			notification();
	});

	function resizeCall(){
		pageCalculations();
		
		$('.swiper-container.initialized[data-slides-per-view="responsive"]').each(function(){
			var thisSwiper = swipers['swiper-'+$(this).attr('id')], $t = $(this), slidesPerViewVar = updateSlidesPerView($t);
			thisSwiper.params.slidesPerView = slidesPerViewVar;
			thisSwiper.reInit();
			var paginationSpan = $t.find('.pagination span');
			var paginationSlice = paginationSpan.hide().slice(0,(paginationSpan.length+1-slidesPerViewVar));
			if(paginationSlice.length<=1 || slidesPerViewVar>=$t.find('.swiper-slide').length) $t.addClass('pagination-hidden');
			else $t.removeClass('pagination-hidden');
			paginationSlice.show();
			updateSlidesPerView(this);
		});
			var a = $(window).height() - 70;
			$("#one").css("max-height",a + "px");
	}

	/*=====================*/
	/* 06 - swiper sliders */
	/*=====================*/
	function initSwiper(){
		var initIterator = 0;
		$('.swiper-container').each(function(){								  
			var $t = $(this);								  

			var index = 'swiper-unique-id-'+initIterator;

			$t.addClass('swiper-'+index + ' initialized').attr('id', index);
			$t.find('.pagination').addClass('pagination-'+index);

			var autoPlayVar = parseInt($t.attr('data-autoplay'));
			var centerVar = parseInt($t.attr('data-center'));
			var simVar = ($t.closest('.circle-description-slide-box').length)?false:true;

			var slidesPerViewVar = $t.attr('data-slides-per-view');
			if(slidesPerViewVar == 'responsive'){
				slidesPerViewVar = updateSlidesPerView($t);
			}
			else slidesPerViewVar = parseInt(slidesPerViewVar);

			var loopVar = parseInt($t.attr('data-loop'));
			var speedVar = parseInt($t.attr('data-speed'));

			var slidesPerGroup = parseInt($t.attr('data-slides-per-group'));
			if(!slidesPerGroup){slidesPerGroup=1;}			

			swipers['swiper-'+index] = new Swiper('.swiper-'+index,{
				speed: speedVar,
				pagination: '.pagination-'+index,
				loop: loopVar,
				paginationClickable: true,
				autoplay: autoPlayVar,
				slidesPerView: slidesPerViewVar,
				slidesPerGroup: slidesPerGroup,
				keyboardControl: true,
				calculateHeight: true, 
				simulateTouch: simVar,
				centeredSlides: centerVar,
				roundLengths: true,
				onSlideChangeEnd: function(swiper){
					var activeIndex = (loopVar===1)?swiper.activeLoopIndex:swiper.activeIndex;
					var qVal = $t.find('.swiper-slide-active').attr('data-val');
					$t.find('.swiper-slide[data-val="'+qVal+'"]').addClass('active');
				},
    {
        title: 'Cow',
        description: 'Another Animal',
					}
  ]
    ;
        $('.ui.search').search({
            apiSettings: {
                url: '/Home/search?id={query}'
				},
            fields: {
                results: 'UserList',
                title: 'UserName',
                url: ''
				},
            minCharacters: 3            
	})
    ;
});