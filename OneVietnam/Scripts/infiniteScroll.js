﻿var page = 1,
    inCallback = false,
    hasReachedEndOfInfiniteScroll = false;

var scrollHandler = function () {
    if (hasReachedEndOfInfiniteScroll === false &&
            ($(window).scrollTop() === $(document).height() - $(window).height())) {
        loadMoreToInfiniteScrollTable(moreRowsUrl);
    }
}

var ulScrollHandler = function () {
    if (hasReachedEndOfInfiniteScroll === false &&
            ($(window).scrollTop() === $(document).height() - $(window).height())) {
        loadMoreToInfiniteScrollUl(moreRowsUrl);
    }
}

function loadMoreToInfiniteScrollUl(loadMoreRowsUrl) {
    if (page > -1 && !inCallback) {
        inCallback = true;
        page++;
        $("div#loading").show();
        $.ajax({
            type: 'GET',
            url: loadMoreRowsUrl,
            data: "pageNum=" + page,
            success: function (data, textstatus) {
                if (data != '') {
                    $("ul.infinite-scroll").append(data);
                }
                else {
                    page = -1;
                }

                inCallback = false;

                $("div#loading").hide();
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
            }
        });
    }
}

function loadMoreToInfiniteScrollTable(loadMoreRowsUrl) {
    if (page > -1 && !inCallback) {
        inCallback = true;
        page++;
        $("#loading").show();
        $.ajax({
            type: 'GET',
            url: loadMoreRowsUrl,
            data: "pageNum=" + page,
            success: function (data, textstatus) {
                if (data != '') {
                    var $items = $(data);

                    $('.ui.link.cards').append($items);
                    $('.grids').isotope('appended', $items);
                    $('.grids').isotope('layout');
                 
                   
                }
                else {
                    page = -1;
                }
                inCallback = false;
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                console.log("Can not load more data infiniteScroll");
            }
        });
    }
}

function showNoMoreRecords() {
    hasReachedEndOfInfiniteScroll = true;
}