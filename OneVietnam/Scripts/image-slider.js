
function plusSlides(n, slideIndex) {
    showSlides(slideIndex += n);
}

function currentSlide(n, slideIndex) {
    showSlides(slideIndex = n);
}

function showSlides(n, slideIndex) {
    var i;
    var slides = document.getElementsByClassName("side");
    var dots = document.getElementsByClassName("dot");
    if (n > slides.length) {
        slideIndex = 1;
    }
    if (n < 1) {
        slideIndex = slides.length;
    }
    for (i = 0; i < slides.length; i++) {
        slides[i].style.display = "none";
    }
    for (i = 0; i < dots.length; i++) {
        dots[i].className = dots[i].className.replace(" active", "");
    }
    slides[slideIndex - 1].style.display = "block";
    dots[slideIndex - 1].className += " active";
}




/*<div class="slideshow-container">
                                <div class="mySlides fade">
                                    
                                    <img src="/Images/dat.JPG" style="width: 100%">
                                    <div class="text">Caption Text</div>
                                </div>
                                <div class="mySlides fade">
                                    
                                    <img src="/Images/heart40.png" style="width: 100%">
                                    <div class="text">Caption Two</div>
                                </div>
                                <div class="mySlides fade">                                    
                                    <img src="/Images/m1.png" style="width: 100%">
                                    <div class="text">Caption Three</div>
                                </div>
                                <a class="prev" onclick="plusSlides(-1)">❮</a>
                                <a class="next" onclick="plusSlides(1)">❯</a>
                            </div>
                            <div style="text-align:center">
                                <span class="dot" onclick="currentSlide(1)"></span>
                                <span class="dot" onclick="currentSlide(2)"></span>
                                <span class="dot" onclick="currentSlide(3)"></span>
                            </div>
                            <script>
var slideIndex = 1;
showSlides(slideIndex);

function plusSlides(n) {
  showSlides(slideIndex += n);
}

function currentSlide(n) {
  showSlides(slideIndex = n);
}

function showSlides(n) {
  var i;
  var slides = document.getElementsByClassName("mySlides");
  var dots = document.getElementsByClassName("dot");
  if (n > slides.length) {slideIndex = 1}
  if (n < 1) {slideIndex = slides.length}
  for (i = 0; i < slides.length; i++) {
      slides[i].style.display = "none";
  }
  for (i = 0; i < dots.length; i++) {
      dots[i].className = dots[i].className.replace(" active", "");
  }
  slides[slideIndex-1].style.display = "block";
  dots[slideIndex-1].className += " active";
}
                            </script>*/