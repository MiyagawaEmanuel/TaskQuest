$("#showPaletteOnly").spectrum({
    showPaletteOnly: true,
    showPalette:true,
    color: 'green',
    change: function (color) {
        $("#Cor").val(color);
    },
    palette: [
        ['#106494', '#2E8B57', '#7A378B'],
        ['#CD2626', '#4F4F4F', '#CD950C']
    ]
});