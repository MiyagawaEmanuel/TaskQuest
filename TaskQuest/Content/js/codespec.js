$("#showPaletteOnly").spectrum({
    showPaletteOnly: true,
    showPalette:true,
    color: 'green',
    change: function (color) {
        $("#Cor").val(color.toName());
    },
    palette: [
        ['black', 'white', 'blanchedalmond',
        'rgb(255, 128, 0);', 'hsv 100 70 50'],
        ['red', 'yellow', 'green', 'blue', 'violet']
    ]
});