var app = angular.module('ComixAdd', ['ngTagsInput']);
app.controller('switchTemplateController', function ($scope) {
    $scope.items = ['skew', 'triad', 'tetrad'];
    //$scope.$watch('selection', function () { make_it_draggable(); })
    $scope.selection = $scope.items[0];
    $scope.remove = function ($event) {
        angular.element($event.target).parent().parent().remove();
    }
});

app.directive('fileDropzone', function () {
        return {
            restrict: 'A',
            scope: {
                file: '=',
                fileName: '='
            },
            link: function (scope, element, attrs) {

                var checkSize, isTypeValid, processDragOverOrEnter, validMimeTypes;

                processDragOverOrEnter = function (event) {
                    if (event != null) {
                        event.preventDefault();
                    }
                    event = event.originalEvent;
                    event.dataTransfer.effectAllowed = 'copy';
                    return false;
                };
                validMimeTypes = attrs.fileDropzone;
                checkSize = function (size) {
                    var _ref;
                    if (((_ref = attrs.maxFileSize) === (void 0) || _ref === '') || (size / 1024) / 1024 < attrs.maxFileSize) {
                        return true;
                    } else {
                        alert("File must be smaller than " + attrs.maxFileSize + " MB");
                        return false;
                    }
                };
                isTypeValid = function (type) {
                    if ((validMimeTypes === (void 0) || validMimeTypes === '') || validMimeTypes.indexOf(type) > -1) {
                        return true;
                    } else {
                        alert("Invalid file type.  File must be one of following types " + validMimeTypes);
                        return false;
                    }
                };
                element.bind('dragover', processDragOverOrEnter);
                element.bind('dragenter', processDragOverOrEnter);
                return element.bind('drop', function (event) {
                    var file, name, reader, size, type;
                    if (event != null) {
                        event.preventDefault();
                    }
                    event = event.originalEvent;
                    reader = new FileReader();
                    reader.onload = function (evt) {
                        if (checkSize(size) && isTypeValid(type)) {
                            return scope.$apply(function () {
                                scope.file = evt.target.result;
                                if (angular.isString(scope.fileName)) {
                                    return scope.fileName = name;
                                }
                            });
                        }
                    };
                    try{
                        file = event.dataTransfer.files[0];

                        name = file.name;
                        type = file.type;
                        size = file.size;
                        reader.readAsDataURL(file);
                    }
                    catch (e) {
                        return false;
                    }
                    return false;
                });
            }
        };
    });

app.controller('dragndrop', function ($scope) {
    $scope.image = null;
    $scope.imageFileName = '';
    $scope.remove = function ($event) {
        angular.element($event.target).parent().remove();
    }
});

app.controller("pagesController", function ($scope, $http) {
    $scope.page = '/Comix/ComixPage';
    $scope.drgbl = make_it_draggable;
    $scope.$watch(function () {
        return angular.element;
    }, function () {
            $scope.$evalAsync(function () {
                $scope.drgbl();
            });
    });
    $scope.tags = [];
    $scope.loadTags = function (query) {
        return $http.get("/Comix/GetTagsForAutocomplete/" + query);
    };
    $scope.save = function () {
        var comix = $http.post('/Comix/ReceiveComix', getComix($scope));
        comix.success(function(data, status, headers, config) {
            window.location.href = data;
        });
        comix.error(function(data, status, headers, config) {
            //alert( "failure message: " + JSON.stringify({data: data}));
            alert("Unable to upload comix");
        });		
    }
    }).directive("comixManager", function ($compile) {
    return {
        templateUrl: '/Comix/ComixPage',
        restrict: 'E',
        link: function (scope, elm) {
            scope.add = function () {
                angular.element("#pages").append($compile('<comix-manager></comix-manager>')(scope));
                //scope.drgbl();
            }
        }
    };
    });

//app.directive('drgbl', function($timeout) {
//    return {
//        link: function(scope, element, attr) {
//            $timeout(function() {
//                make_it_draggable();
//                });
//        }
//    }
//});

function getComix(scope) {
    var pages = [];
    for(let page of $("#pages").children("comix-manager").toArray()) {
        pages.push(getPage(page));
    }
    return { 
        "pages": pages,
        "tags" : scope.tags, 
        "name": $("#Name").val()
    };
}

function getPage(page) {
    var template = $(page).find("[ng-switch-when]").attr("ng-switch-when");
    //page.find("#comic_wrapper_" + template)
    var frame_images = [];
    for(let image of $(page).find(".frame-image").toArray()) {
        frame_images.push(getImage(image));
    }
    return {
        "template": template,
        "frameimages": frame_images
    }
}

function getImage(image) {
    var bg = $(image).css("background-image").replace('url(\"', '').replace('\")', '');
    var top = $(image).css("top");
    var left = $(image).css("left");
    var width = $(image).css("width");
    var height = $(image).css("height");

    var balloons = [];
    for(let balloon of $(image).find(".talkbubble").toArray()) {
        balloons.push(getBalloon(balloon));
    }

    return {
        "backgroundimage": bg,
        "top": top,
        "left": left,
        "width": width,
        "height": height,
        "balloons": balloons
    }
}

function getBalloon(balloon) {
    var text = $(balloon).find("textarea").val();
    var top = $(balloon).css("top");
    var left = $(balloon).css("left");
    var width = $(balloon).css("width");
    var height = $(balloon).css("height");

    return {
        "text": text,
        "top": top,
        "left": left,
        "width": width,
        "height": height
    }
}


function make_it_draggable() {
    $(".frame-image").draggable({
        //containment: "parent"
    }).resizable();
    $('.frame-image').bind('mousewheel', function (e) {
        if (e.originalEvent.deltaY < 0) {
            $(this).css("width", "+=16");
            $(this).css("height", "+=16");
        }
        else {
            $(this).css("width", "-=16");
            $(this).css("height", "-=16");
        }
        return false;
    });

    $("#balloons_panel .talkbubble").draggable(
        { helper: "clone" }).resizable({ containment: "parent" });

    $(".frame-image").droppable({
        accept: "#balloons_panel .talkbubble",
        drop: function (event, ui) {
            var clone = ui.draggable.clone();
            clone.draggable({
                containment: "parent"
            });

            clone.attr("class", "talkbubble");
            clone.find(".ui-resizable-handle").remove();
                
            clone.css("top", ui.offset.top - $(this).offset().top);
            clone.css("left", ui.offset.left - $(this).offset().left);
            clone.css("position", "absolute");

            clone.find('.delete-cross').click(function () { clone.remove(); });

            $(this).append(clone);
        }
    });
};