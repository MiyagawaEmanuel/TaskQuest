$("#Cor").spectrum({
    showPaletteOnly: true,
    showPalette: true,
    change: function (color) {
        $("#Cor").val(color.toHex());
    },
    color: '#106494',
    palette: [
        ['#106494', '#2E8B57', '#7A378B'],
        ['#CD2626', '#4F4F4F', '#CD950C']
    ]
});