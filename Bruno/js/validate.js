<script>  

        $(document).ready(function() {
            $("input[name='campo']").prop('disabled', true);
            $("textarea[name='campo']").prop('disabled', true);
            $("select[name='campo']").prop('disabled', true);
        });

        $("input[type='button']").click(function() {
            $("input[name='campo']").prop('disabled', false);
            $("textarea[name='campo']").prop('disabled', false);
            $("select[name='campo']").prop('disabled', false);
         });

</script>