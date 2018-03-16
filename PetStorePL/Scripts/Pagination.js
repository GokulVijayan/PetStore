function PagerClick(index) {
    document.getElementById("hfCurrentPageIndex").value = index;
    document.forms[1].submit();
}