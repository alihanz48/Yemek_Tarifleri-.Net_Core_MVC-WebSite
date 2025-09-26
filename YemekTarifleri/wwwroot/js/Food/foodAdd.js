$(document).ready(function () {
  var count = 0;
  $("#addIngredient").click(function () {
    if ($("#ingredientText").val().length >= 3) {
      $('div[name="ing_Alert"]').remove();
      count++;
      $("#ingredients").append(`
          <div id="Ingredient_${count}" name="IngredientDiv">
            <li class="list-group-item d-flex justify-content-between">
              <div class="d-flex" id="div_${count}">
                <i class="bi bi-arrow-right"></i>
                <p class="mb-0 ms-2" id="text_${count}">${$("#ingredientText").val()}</p>
                <input type="hidden" id="iinpt_${count}" name="ingredientsInput" value="${$("#ingredientText").val()}">
              </div>
              <div>
                <button class="btn btn-primary delIngredient" type="button"><i
                    class="bi bi-trash3"></i></button>
                <button class="btn btn-danger editIngredient" data-id="${count}" type="button"><i
                    class="bi bi-pencil"></i></button>
              </div>
            </li>
          </div>`
      );
      $("#ingredientText").val('');
    } else {
      $('div[name="ing_Alert"]').remove();
      $("#ingDiv").append(`
          <div class="alert alert-danger" name="ing_Alert" role="alert">
            İçindekiler en az 3 karakterden oluşmalıdır !
          </div>
        `);
    }

  });

  var selectId = 0;
  $(document).on("click", ".delIngredient", function () {
    $(this).closest('div[name="IngredientDiv"]').remove();
  });

  var itext = "";
  $(document).on("click", ".editIngredient", function () {
    selectId = $(this).data("id");
    itext = $("#text_" + selectId).text();

    $("#text_" + selectId).remove();
    $("#div_" + selectId).append(`<input class="form-control" id="newIngredients" value="${itext}">`);
    $("#div_" + selectId).append(`<button class="btn btn-success editOKIngredient" id="confirmBtn" data-id="${count}" type="button"><i class="bi bi-check2"></i></button>`);
  });

  $(document).on("click", ".editOKIngredient", function () {
    itext = $("#newIngredients").val()
    $("#newIngredients").remove();
    $("#confirmBtn").remove();
    $("#div_" + selectId).append(`<p class="mb-0 ms-2" id="text_${selectId}">${itext}</p>`);
    $("#iinpt_" + selectId).remove();
    $("#div_" + selectId).append(`<input type="hidden" id="iinpt_${selectId}" name="ingredientsInput" value="${itext}">`);
  });

  let stepcount = 0;
  $("#Addsteps").click(function () {
    if ($("#stepText").val().length >= 5) {
      $('div[name="step_Alert"]').remove();
      stepcount++;
      $("#steps").append(`
          <div id="step_${stepcount}">
            <li class="list-group-item d-flex justify-content-between">
              <div class="d-flex" id="sdiv_${stepcount}">
                <i class="bi bi-arrow-right"></i>
                <p class="mb-0 ms-2" id="stext_${stepcount}">${$("#stepText").val()}</p>
                <input type="hidden" id="sinpt_${stepcount}" name="stepsInput" value="${$("#stepText").val()}">
              </div>
              <div>
                <button class="btn btn-primary delStep" data-id="${stepcount}" type="button"><i
                    class="bi bi-trash3"></i></button>
                <button class="btn btn-danger editStep" data-id="${stepcount}" type="button"><i
                    class="bi bi-pencil"></i></button>
              </div>
            </li>
          </div>`
      );
      $("#stepText").val('');
    } else {
      $('div[name="step_Alert"]').remove();
      $("#stepDiv").append(`
          <div class="alert alert-danger" name="step_Alert" role="alert">
            Adım en az 5 karakterden oluşmalıdır !
          </div>
        `)
    }
  });

  var stepselectid = 0;
  $(document).on("click", ".delStep", function () {
    stepselectid = $(this).data("id");
    $("#step_" + stepselectid).remove();
  });

  var itext = "";
  $(document).on("click", ".editStep", function () {
    stepselectid = $(this).data("id");
    itext = $("#stext_" + stepselectid).text();
    $("#stext_" + stepselectid).remove();
    $("#sdiv_" + stepselectid).append(`<input class="form-control" id="newStep" value="${itext}">`);
    $("#sdiv_" + stepselectid).append(`<button class="btn btn-success editOKStep" id="confirmBtn" data-id="${stepselectid}" type="button"><i class="bi bi-check2"></i></button>`);
  });

  $(document).on("click", ".editOKStep", function () {
    itext = $("#newStep").val()
    $("#newStep").remove();
    $("#confirmBtn").remove();
    $("#sdiv_" + stepselectid).append(`<p class="mb-0 ms-2" id="stext_${stepselectid}">${itext}</p>`);
    $("#sinpt_" + stepselectid).remove();
    $("#sdiv_" + stepselectid).append(`<input type="hidden" id="sinpt_${stepselectid}" name="stepsInput" value="${itext}">`);
  });

  let selectedFiles = [];

  $("#images").on("change", function () {
    $('div[name="photoDiv"]').remove();
    $('div[name="img_alert"]').remove();

    selectedFiles = [...this.files];

    if (selectedFiles.length > 0 && selectedFiles.length < 6) {
      [...selectedFiles].forEach((file, idx) => {
        const url = URL.createObjectURL(file);

        $("#f-images").append(`
                <div class="col-4 position-relative" name="photoDiv" data-index="${idx}">
                    <div class="photo-box border rounded p-1 mb-2">
                        <button type="button" class="btn btn-sm btn-danger position-absolute top-0 end-0 m-1 delete-image" title="Sil" style="z-index: 2;">&times;</button>
                        <img src="${url}" style="width: 100%; height: auto;" class="img-fluid rounded" />
                    </div>
                </div>
            `);
      });
    } else {
      $("#f-images").append(`
            <div class="alert alert-danger w-100" name="img_alert" role="alert">
                En az 1, en fazla 5 resim seçebilirsiniz!
            </div>
        `);
    }
  });

  // Silme butonuna tıklanınca görsel kutusunu kaldır
  $("#f-images").on("click", ".delete-image", function () {
    const photoDiv = $(this).closest('div[name="photoDiv"]');
    const index = photoDiv.data("index");
    selectedFiles.splice(index, 1);
    photoDiv.remove();
    redrawImages();
  });

  function redrawImages() {
    $('div[name="photoDiv"]').remove();
    selectedFiles.forEach((file, idx) => {

      const url = URL.createObjectURL(file);

      $("#f-images").append(`
            <div class="col-4 position-relative" name="photoDiv" data-index="${idx}">
                <div class="photo-box border rounded p-1 mb-2">
                    <button type="button" class="btn btn-sm btn-danger position-absolute top-0 end-0 m-1 delete-image" title="Sil" style="z-index: 2;">&times;</button>
                    <img src="${url}" style="width: 100%; height: auto;" class="img-fluid rounded" />
                </div>
            </div>
        `);
    });
  }

  $("#SaveFood").click(function (event) {
    event.preventDefault();
    var check = [];

    var formdata = new FormData();

    if ($("#foodName").val().length < 5) {
      $('div[name="name_alert"]').remove();
      $("#fname").append(`
          <div class="alert alert-danger" name="name_alert" role="alert">
            Yemek ismi en az 5 karakterden oluşmalıdır !
          </div>`
      );
      check[0] = false;
    } else {
      check[0] = true;
      $('div[name="name_alert"]').remove();
    }

    if ($("#foodDescription").val().length > 12 && $("#foodDescription").val().length < 30) {
      $('div[name="desc_alert"]').remove();
      check[1] = true;
    } else {
      $('div[name="desc_alert"]').remove();
      $("#fdesc").append(`
          <div class="alert alert-danger" name="desc_alert" role="alert">
            Açıklama metni 12 karakterden kısa ve 30 karakterden uzun olamaz !
          </div>`
      );
      check[1] = false;
    }

    formdata.append("isim", $("#foodName").val());
    formdata.append("aciklama", $("#foodDescription").val());

    var ingredientsList = $('input[name="ingredientsInput"]').map(function () {
      return $(this).val();
    }).get();
    if (ingredientsList.length < 1) {
      $('div[name="ing_Alert"]').remove();
      $("#ingDiv").append(`
          <div class="alert alert-danger" name="ing_Alert" role="alert">
            En az 1 bileşen eklenmelidir!
          </div>
        `)
      check[2] = false;
    } else {
      check[2] = true;
    }

    var stepsList = $('input[name="stepsInput"]').map(function () {
      return $(this).val();
    }).get();
    if (stepsList.length < 1) {
      $('div[name="step_Alert"]').remove();
      $("#stepDiv").append(`
          <div class="alert alert-danger" name="step_Alert" role="alert">
            En az 1 adım eklenmelidir!
          </div>
        `)
      check[3] = false;
    } else {
      check[3] = true;
    }

    var ftypeIDs = $('input[name="ftypeIDs"]:checked').map(function () {
      return $(this).val();
    }).get();

    ingredientsList.forEach((ingredient) => {
      formdata.append("ingredients", ingredient);
    });

    stepsList.forEach((step) => {
      formdata.append("steps", step);
    });

    ftypeIDs.forEach((id) => {
      formdata.append("ftypeIDs", id);
    });

    if (selectedFiles.length > 0 && selectedFiles.length < 6) {
      $('div[name="img_alert"]').remove();
      [...selectedFiles].forEach((file, idx) => {
        formdata.append("img", file);
      });

      check[4] = true;
    } else {
      $('div[name="img_alert"]').remove();
      $("#fimages").append(`
          <div class="alert alert-danger" name="img_alert" role="alert">
            En az 1 resim en fazla 5 resim eklenebilir !
          </div>`
      );
      check[4] = false;
    }

    formdata.append("userID", $("#userid").val());

    if (check.indexOf(false) == -1 ? true : false) {
      $.ajax({
        type: 'POST',
        url: $("#addFoodUrl").val(),
        dataType: 'json',
        data: formdata,
        contentType: false,
        processData: false,
        success: function (result) {
          $("#addCheck").append(`
              <div class="alert alert-success" role="alert">
                Yemeğiniz başarıyla eklendi !  Yönlendiriliyorsunuz...
              </div>
             `);
          setTimeout(function () {
            window.location.assign("/Users/MyAccount");
          }, 3000);
        }
      })
    }

  });

});



