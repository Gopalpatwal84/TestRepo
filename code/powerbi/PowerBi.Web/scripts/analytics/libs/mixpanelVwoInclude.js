var _vis_opt_queue = window._vis_opt_queue || [], _vis_counter = 0, mixpanel = window.mixpanel || [];
_vis_opt_queue.push(function() {
    try {
        if(!_vis_counter) {
            var _vis_data = {},_vis_combination,_vis_id,_vis_l=0;
            for(;_vis_l<_vwo_exp_ids.length;_vis_l++) {
                _vis_id = _vwo_exp_ids[_vis_l];
                if(_vwo_exp[_vis_id].ready) {
                    _vis_combination = _vis_opt_readCookie('_vis_opt_exp_'+_vis_id+'_combi');
                    if(typeof(_vwo_exp[_vis_id].combination_chosen) != "undefined")
                        _vis_combination = _vwo_exp[_vis_id].combination_chosen;
                    if(typeof(_vwo_exp[_vis_id].comb_n[_vis_combination]) != "undefined") {
                        _vis_data['VWO-Test-ID-'+_vis_id] = _vwo_exp[_vis_id].comb_n[_vis_combination];
                        _vis_counter++;
                    }
                }
            }
            // Use the _vis_data object created above to fetch the data,
            // key of the object is the Test ID and the value is Variation Name
            if(_vis_counter) {
            mixpanel.push(['register_once', _vis_data]);
            mixpanel.track("VWO", _vis_data);
          }
        }
    }
    catch(err) {};
});