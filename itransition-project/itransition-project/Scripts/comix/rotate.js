// Allow rotation of dom objects
(function( $ ){
  var parseMatrix = function(mstring) {
    mstring = mstring.slice(7, -1);
    m = mstring.split(',');
    return m;
  }
  var multMatrix = function(m1, m2) {
    m = [];
    m.push(m1[0]*m2[0]+m1[2]*m2[1]);
    m.push(m1[0]*m2[2]+m1[2]*m2[3]);
    m.push(m1[1]*m2[0]+m1[3]*m2[1]);
    m.push(m1[1]*m2[2]+m1[3]*m2[3]);
    return m;
  }
  var mToString = function(m) {
    mstring = "matrix(";
    for (mi in m) {
      mstring += m[mi]+", ";
    }
    mstring = mstring.slice(0, -2)+")";
    return mstring;
  }
  
  $.fn.rotate = function(degrees, flipped){
    if (flipped != undefined && !flipped) degrees = -degrees; 
    deg2radians = Math.PI * 2 / 360;
    rad = degrees * deg2radians ;
    costheta = Math.cos(rad);
    sintheta = Math.sin(rad);
    fM = [-1, 0, 0, 1] 

    M = [costheta, -sintheta, sintheta, costheta];
    if (flipped != undefined && flipped) M = multMatrix(fM, M);
    M.push(0);
    M.push(0);
    
    M11 = costheta;
    M12 = -sintheta;
    M21 = sintheta;
    M22 = costheta;

    if ($.browser.msie) {
      // This fix unearthed from:
      // http://msdn.microsoft.com/en-us/library/ms533014%28v=vs.85%29.aspx
      // A simple explanation that [MXX] uses the sine and cosine of radians
      // instead of degrees would have sped up the search quite a bit... 
      // But why would we want adequate and verbose documentation??
      // Who enjoys actually getting work done anyway?? Srsly...
      
      msUglyStepdaughterCode = "progid:DXImageTransform.Microsoft.Matrix(";
      msUglyStepdaughterCode += "M11=" + M11 + ", M12=" + M12 + ", M21=" + M21 + ", M22=" + M22;
      msUglyStepdaughterCode += ", sizingMethod='auto expand')"
      
      this.css("-ms-transform","rotate(" + degrees + "deg)");
      this.css("filter",msUglyStepdaughterCode);
      this.css("zoom","1");
    } else if ($.browser.webkit) {
      //this.css("-webkit-transform","rotate(" + degrees + "deg)");
      this.css("-webkit-transform",mToString(M));
    } else if ($.browser.opera) {
      this.css("-o-transform",mToString(M));
    } else if ($.browser.mozilla) {
      this.css("-moz-transform",mToString(M));
    } else {
      this.css("transform",mToString(M));
    }
    return this;
  };
})( jQuery);