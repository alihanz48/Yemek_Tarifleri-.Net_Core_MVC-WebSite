$(document).ready(function () {

    var formdata = new FormData();
    var selectedImages = [];

    $(document).on("click", ".delete-image", function () {
        if ($(this).closest('#f-images').find('div[name="photoDiv"]').length == 1) {
            alert("Tüm resimler silinemez!!!");
        } else {
            delete selectedImages[$(this).closest('div[name="photoDiv2"]').data("id")];
            $(this).closest('div[name="photoDiv"]').remove();
            formdata.append("deletedImg", $(this).closest('div[name="photoDiv"]').find(".deletedImg").val());
        }

    })


    $(document).on("change", "#images", function () {
        selectedImages = [...this.files];
        [...this.files].forEach((img) => {

            const url = URL.createObjectURL(img);
            $("#f-images").append(`
            <div class="col-4 position-relative" name="photoDiv">
                <div class="photo-box border rounded p-1 mb-2" name="photoDiv2" data-id="${selectedImages.indexOf(img)}">
                    <button type="button" class="btn btn-sm btn-danger position-absolute top-0 end-0 m-1 delete-image" title="Sil" style="z-index: 2;">&times;</button>
                    <img src="${url}" style="width: 100%; height: auto;" class="img-fluid rounded" />
                </div>
            </div>
               `);
        });

    });

    $(document).on("click", ".delIngredient", function () {
        var ingredientDiv = $(this).closest('.ingredient-item');

        if (ingredientDiv.data("id") != null) {
            formdata.append("deletedIngredients", ingredientDiv.data("id"));
        }

        ingredientDiv.remove();
    })

    $(document).on("click", ".editIngredient", function () {


        let ingredientDiv = $(this).closest("div[data-id]");
        let ingredientText = ingredientDiv.find('input[name="IngredientsInput"]').val();


        ingredientDiv.find('p[name="IngredientsText"]').remove();
        ingredientDiv.find('input[name="IngredientsInput"]').remove();
        ingredientDiv.find(".edit-area").append(`<input class="form-control" name="newIngredient" value="${ingredientText}">`);
        ingredientDiv.find(".edit-area").append(`<button class="btn btn-success editOKIngredient"><i class="bi bi-check2"></i></button>`);
        formdata.append("ingredientIDs", ingredientDiv.data("id"));
    })

    $(document).on("click", ".editOKIngredient", function () {

        let ingredientDiv = $(this).closest("div[data-id]");
        var newText = ingredientDiv.find('input[name="newIngredient"]').val();

        ingredientDiv.find(".edit-area").append(`
              <p class="mb-0 ms-2" name="IngredientsText">${newText}</p>
              <input type="hidden" name="IngredientsInput" value="${newText}">
            `);
        ingredientDiv.find('input[name="newIngredient"]').remove();
        ingredientDiv.find(".editOKIngredient").remove();
        formdata.append("newIng", newText);

    })



    $(document).on("click", ".delStep", function () {
        var stepDiv = $(this).closest('.step-item');
        if (stepDiv.data("id") != null) {
            formdata.append("deletedStep", stepDiv.data("id"));
        }
        stepDiv.remove();
    })

    $(document).on("click", ".editStep", function () {

        var stepDiv = $(this).closest('div[data-id]');
        var stepText = stepDiv.find('input[name="stepsInput"]').val();

        stepDiv.find('p[name="stepsText"]').remove();
        stepDiv.find('input[name="stepsInput"]').remove();

        stepDiv.find(".edit-area").append(`<input class="form-control" name="newStepsInput" value="${stepText}">`);
        stepDiv.find(".edit-area").append(`<button class="btn btn-success editOKStep"><i class="bi bi-check2"></i></button>`);

        formdata.append("stepIDs", stepDiv.data("id"));

    })

    $(document).on("click", ".editOKStep", function () {

        var stepDiv = $(this).closest('div[data-id]');
        var newStepText = stepDiv.find('input[name="newStepsInput"]').val();

        stepDiv.find(".edit-area").append(`
              <p class="mb-0 ms-2" name="stepsText">${newStepText}</p>
              <input type="hidden" name="stepsInput" value="${newStepText}">
            `);

        stepDiv.find('input[name="newStepsInput"]').remove();
        stepDiv.find(".editOKStep").remove();
        formdata.append("newStep", newStepText);
    })

    $("#addIngredient").click(function () {
        var ing = $("#ingredientText").val();
        formdata.append("addIng", ing);

        $('#ingredients').append(`<div class="ingredient-item">
                                                  <div>
                            <li class="list-group-item d-flex justify-content-between">
                                <div class="d-flex edit-area">
                                    <i class="bi bi-arrow-right"></i>
                                    <p class="mb-0 ms-2" name="IngredientsText">${ing}</p>
                                    <input type="hidden" name="IngredientsInput" value="${ing}">
                                </div>
                                <div>
                                    <button class="btn btn-primary delIngredient" type="button"><i
                                            class="bi bi-trash3"></i></button>
                                </div>
                            </li>
                        </div>
            `);

        $("#ingredientText").val('');
    });

    $("#Addsteps").click(function () {
        var step = $("#stepText").val();
        formdata.append("addStep", step);

        $("#steps").append(`<div class="step-item">
                            <li class="list-group-item d-flex justify-content-between">
                                <div class="d-flex" id="sdiv">
                                    <i class="bi bi-arrow-right"></i>
                                    <p class="mb-0 ms-2" id="stext">${step}</p>
                                    <input type="hidden" id="sinpt" name="stepsInput" value="${step}">
                                </div>
                                <div>
                                    <button class="btn btn-primary delStep" type="button"><i
                                            class="bi bi-trash3"></i></button>
                                </div>
                            </li>
                        </div>`);
        $("#stepText").val(' ');
    });



    $("#EditFood").click(function (event) {
        event.preventDefault();
        formdata.append("url", $("#url").val());
        formdata.append("FoodName", $("#foodName").val());
        formdata.append("Aciklama", $("#foodDescription").val());

        //formdata addIng ile yeni ingredientler kayıt ediliyor.
        //formdata addStep ile yeni stepler kayıt ediliyor.

        $('input[name="ftypeIDs"]:checked').map(function () {
            formdata.append("ftypes", $(this).val());
        }).get();

        selectedImages.forEach((img) => {
            formdata.append("img", img);
        });



        $.ajax({
            type: 'POST',
            url: $("#foodEditUrl").val(),
            dataType: 'json',
            data: formdata,
            contentType: false,
            processData: false,
            success: function (result) {
                $("#addCheck").append(`
              <div class="alert alert-success" role="alert">
                Yemeğiniz başarıyla güncellendi !  Yönlendiriliyorsunuz...
              </div>
             `);
                setTimeout(function () {
                    window.location.assign("/Users/MyAccount");
                }, 1000);
            }
        })
    });


});




