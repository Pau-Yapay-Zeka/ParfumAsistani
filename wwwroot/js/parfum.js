
    let currentStep = 1;
    let selectedNotalar = [];
    let selectedParfums = [];
    
    let currentShownParfums = [];
    let rejectedParfums = [];
    
    const API_BASE = '';

    function showStep(n) {
        document.querySelectorAll('.step').forEach(s => s.classList.add('d-none'));
        document.getElementById(typeof n === 'string' ? n : 'step-' + n).classList.remove('d-none');
        currentStep = n;
        if (n === 2) loadNotalar();
        
        document.getElementById('error-step-1').classList.add('d-none');
        document.getElementById('error-step-2').classList.add('d-none');
    }

    function goNext(n) {
        if (n === 2) {
            const name = document.getElementById('input-name').value.trim();
            const age = document.getElementById('input-age').value;
            const gender = document.getElementById('input-gender').value;
            
            if (!name || !age || !gender) {
                document.getElementById('error-step-1').classList.remove('d-none');
                return; 
            }
        }
        
        if (n === 3) {
            if (selectedNotalar.length === 0) {
                document.getElementById('error-step-2').classList.remove('d-none');
                return; 
            }
        }
        
        showStep(n);
    }

    async function fetchWithFallback(apiUrl, mockUrl){
        try{
            const res = await fetch(apiUrl, { cache: 'no-store' });
            if(!res.ok) throw new Error('api-failed');
            return await res.json();
        }catch(e){
            console.warn('API fallback to mock', e);
            try {
                const res = await fetch(mockUrl);
                return await res.json();
            } catch(ex) { return []; }
        }
    }

    // YENİ: FLIP EFEKTLİ NOTALAR BURADA EKRANA ÇİZİLİYOR
   async function loadNotalar(){
        const container = document.getElementById('notalar-container');
        if (container.dataset.loaded) return;
        container.innerHTML = '<div class="col-12 text-center text-slate-400 py-10 animate-pulse font-medium">Notalar hazırlanıyor...</div>';
        try{
            const list = await fetchWithFallback(`${API_BASE}/api/parfum/notalar`, './mocks/notalar.json');
            container.innerHTML = '';
            list.forEach(n => {
                // Ekstra boşluklar silindi, ızgara (grid) sisteminin kendi boşluğu kullanılacak
                const col = document.createElement('div'); 
                col.className = 'col-6 col-md-3'; 
                
                const card = document.createElement('div'); 
                card.className = 'flip-card-container w-full'; 
                
                const gelenAciklama = n.aciklama || n.Aciklama; 
                const aciklamaYazisi = gelenAciklama ? escapeHtml(gelenAciklama) : 'Karakteristik bir koku...';

                card.innerHTML = `
                <div class="flip-card-inner">
                    <div class="flip-card-front backface-hidden flex flex-col items-center p-3 rounded-2xl border border-slate-200 bg-white shadow-sm transition-all h-full">
                        <img src="${n.gorselUrl}" class="w-full h-28 object-cover rounded-xl mb-3 shadow-sm" onerror="this.src='https://placehold.co/300x200?text=${n.ad}';this.onerror=null;" />
                        <div class="text-sm font-bold text-slate-700 text-center tracking-wide leading-tight mt-auto mb-1">${n.ad}</div>
                    </div>
                    
                    <div class="flip-card-back backface-hidden flex flex-col items-center justify-center p-4 rounded-2xl border border-red-200 bg-red-50 text-red-700 shadow-sm h-full overflow-y-auto" style="transform: rotateY(180deg);">
                        <div class="font-bold text-sm mb-2 border-b border-red-200 pb-1 w-full text-center tracking-wider">${n.ad}</div>
                        <p class="text-xs font-medium text-center leading-relaxed m-auto">${aciklamaYazisi}</p>
                    </div>
                </div>`;
                
                card.onclick = () => toggleNota(n.ad, card);
                
                if(selectedNotalar.includes(n.ad)) card.classList.add('selected');

                col.appendChild(card); container.appendChild(col);
            });
            container.dataset.loaded='1';
        }catch(e){ container.innerHTML = '<div class="col-12 text-red-600 text-center">Notalar yüklenemedi.</div>';}    
    }
    
    function toggleNota(name, cardEl){ 
        const idx = selectedNotalar.indexOf(name); 
        if (idx===-1){ 
            selectedNotalar.push(name); 
            cardEl.classList.add('selected'); 
            document.getElementById('error-step-2').classList.add('d-none'); 
        } else { 
            selectedNotalar.splice(idx,1); 
            cardEl.classList.remove('selected'); 
        } 
    }

    const inputSearch = document.getElementById('input-search');
    const acList = document.getElementById('autocomplete-list');
    const selectedList = document.getElementById('selected-parfums');
    let debounceTimer;
    
    if(inputSearch){ 
        inputSearch.addEventListener('input', e => { 
            clearTimeout(debounceTimer); 
            debounceTimer = setTimeout(() => doSearch(e.target.value), 300); 
        }); 
    }

    async function doSearch(q){
        acList.innerHTML=''; if(!q||q.trim().length<1) return;
        try{
            const apiUrl = `${API_BASE}/api/parfum/ara?kelime=${encodeURIComponent(q)}`;
            const items = await fetchWithFallback(apiUrl, './mocks/parfums.json');
            const filtered = items.filter(p => (p.ad || '').toLowerCase().includes(q.toLowerCase()));
            filtered.forEach(p => {
                const name = p.ad;
                const itm = document.createElement('button'); itm.type='button'; 
                itm.className='list-group-item w-full text-left px-4 py-3 border-b border-slate-100 hover:bg-red-50 transition-colors focus:bg-red-50 focus:outline-none'; 
                itm.innerHTML = `<div class="font-bold text-slate-800">${name}</div> <div class="text-red-600 text-xs font-semibold uppercase tracking-wider mt-1">${p.marka || ''}</div>`;
                itm.onclick = () => { addParfum(name); acList.innerHTML=''; inputSearch.value=''; };
                acList.appendChild(itm);
            });
        } catch(e){ console.error(e); }
    }

    document.addEventListener('click', e => { 
        if(acList && !acList.contains(e.target) && e.target !== inputSearch) acList.innerHTML=''; 
    });

    document.getElementById('btn-add-custom')?.addEventListener('click', () => { 
        const v = inputSearch.value && inputSearch.value.trim(); 
        if(v){ addParfum(v); inputSearch.value=''; acList.innerHTML=''; } 
    });

    function addParfum(name){ 
        if(!name) return; if(selectedParfums.includes(name)) return; 
        selectedParfums.push(name); 
        
        const li = document.createElement('li'); 
        li.className = 'flex justify-between items-center p-3 bg-white border border-slate-200 rounded-xl shadow-sm hover:shadow-md transition-all'; 
        li.innerHTML = `<span class="font-bold text-slate-700 flex items-center gap-3">
            <div class="w-8 h-8 rounded-full bg-slate-100 flex items-center justify-center text-red-600"><svg xmlns="http://www.w3.org/2000/svg" width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="m2 7 4.41-4.41A2 2 0 0 1 7.83 2h8.34a2 2 0 0 1 1.42.59L22 7"/></svg></div>
            ${escapeHtml(name)}
        </span>`; 
        
        const btn = document.createElement('button'); 
        btn.className='w-8 h-8 flex items-center justify-center rounded-full bg-red-50 text-red-500 hover:bg-red-600 hover:text-white transition-colors'; 
        btn.innerHTML='<svg xmlns="http://www.w3.org/2000/svg" width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M18 6 6 18"/><path d="m6 6 12 12"/></svg>'; 
        btn.onclick=()=>{ selectedParfums = selectedParfums.filter(x=>x!==name); li.remove(); }; 
        li.appendChild(btn); 
        selectedList.appendChild(li); 
    }
    
    function escapeHtml(s){ return String(s).replaceAll('&','&amp;').replaceAll('<','&lt;').replaceAll('>','&gt;'); }

    async function safeParseAPIResponse(apiRes) {
        try {
            let txt = await apiRes.text();
            if (!txt) return [];
            
            console.log("C# VEYA GEMINI'DEN GELEN HAM CEVAP BURADA:", txt);
            
            if (txt.startsWith('"') && txt.endsWith('"')) {
                try { txt = JSON.parse(txt); } catch(err){}
            }

            const arrayMatch = txt.match(/\[[\s\S]*\]/);
            if (arrayMatch) {
                try {
                    const extractedData = JSON.parse(arrayMatch[0]);
                    if (Array.isArray(extractedData) && extractedData.length > 0) {
                        return extractedData;
                    }
                } catch(err) { console.log("Cımbızlama başarısız oldu, düz parse denenecek."); }
            }
            
            let data;
            try { data = JSON.parse(txt); } 
            catch (err) { return []; }
            
            if (data && typeof data === 'object' && !Array.isArray(data)) {
                if(data.error || data.message || !data.ad) return []; 
                return [data]; 
            }
            
            return Array.isArray(data) ? data : [];
        } catch (e) {
            console.error("Yapay Zeka Yanıt Hatası:", e);
            return []; 
        }
    }

    function generateCardHTML(p, idx) {
        if(!p || !p.ad) return ''; 

        return `
        <div class="h-full p-6 rounded-2xl border border-slate-200 bg-white shadow-sm transition-all hover:-translate-y-2 hover:shadow-xl hover:border-red-600/50 flex flex-col relative overflow-hidden group min-h-[300px]">
            
            <div class="absolute top-0 right-0 p-4 opacity-5 group-hover:opacity-10 transition-opacity pointer-events-none">
                <svg xmlns="http://www.w3.org/2000/svg" width="100" height="100" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1" stroke-linecap="round" stroke-linejoin="round" class="text-red-600"><path d="M10 4h4"/><path d="M12 4v4"/><rect x="7" y="8" width="10" height="13" rx="3"/><path d="M12 13v4"/><circle cx="12" cy="13" r="1.5" fill="currentColor"/></svg>
            </div>

            <button onclick="replaceCard(this, '${escapeHtml(p.ad)}', ${idx})" title="Beğenmedim, değiştir" class="absolute top-4 right-4 text-slate-300 hover:text-red-600 hover:bg-red-50 w-10 h-10 rounded-full flex items-center justify-center transition-all z-20 cursor-pointer border border-transparent hover:border-red-100 bg-white shadow-sm">
                <svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round"><path d="M18 6 6 18"/><path d="m6 6 12 12"/></svg>
            </button>

            <div class="w-12 h-12 bg-red-50 rounded-xl flex items-center justify-center text-red-600 font-bold text-xl mb-6 shadow-inner relative z-10">${idx+1}</div>
            
            <h5 class="text-xl font-extrabold text-slate-800 mb-1 pr-10 relative z-10">${escapeHtml(p.ad)}</h5>
            <h6 class="text-sm font-bold text-red-600 mb-4 tracking-wider uppercase relative z-10">${escapeHtml(p.marka || '')}</h6>
            <p class="text-sm text-slate-600 leading-relaxed flex-1 relative z-10 mb-8">${escapeHtml(p.neden || '')}</p>
            
            <a href="${p.link ? escapeHtml(p.link) : `https://www.google.com/search?tbm=shop&q=${encodeURIComponent((p.marka || '') + ' ' + p.ad + ' parfüm')}`}" target="_blank" class="mt-auto relative z-10 w-full py-3 px-4 bg-red-50 hover:bg-red-600 text-red-600 hover:text-white font-bold rounded-xl flex items-center justify-center gap-2 transition-colors border border-red-100 hover:border-red-600">
                Satın Almaya Git
                <svg xmlns="http://www.w3.org/2000/svg" width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M5 12h14"/><path d="m12 5 7 7-7 7"/></svg>
            </a>
        </div>`;
    }

    async function replaceCard(btnElement, oldParfumName, index) {
        const cardContainer = btnElement.closest('.w-full');
        
        if(!rejectedParfums.includes(oldParfumName)) {
            rejectedParfums.push(oldParfumName);
        }

        cardContainer.innerHTML = `
        <div class="h-full p-6 rounded-2xl border border-slate-200 bg-white shadow-sm flex flex-col items-center justify-center min-h-[300px]">
            <div class="loader-ring w-10 h-10 mb-4" style="border-width: 3px;"></div>
            <p class="text-sm font-bold text-slate-600 animate-pulse text-center">Farklı bir öneri aranıyor...</p>
        </div>`;

        const name = document.getElementById('input-name').value.trim();
        const age = parseInt(document.getElementById('input-age').value || '0');
        const gender = document.getElementById('input-gender').value;

        const gizliMesaj = `${name} (NOT: Lütfen öncekileri tekrar önerme: ${rejectedParfums.join(', ')} ve ${currentShownParfums.join(', ')})`;
        const payload = { Ad: gizliMesaj, Yas: age, Cinsiyet: gender, Notalar: selectedNotalar, SevdigiParfumler: selectedParfums };

        try {
            const apiRes = await fetch(`${API_BASE}/api/parfum/oneri`, { 
                method: 'POST', 
                headers: { 'Content-Type':'application/json' }, 
                body: JSON.stringify(payload) 
            });
            
            let data = await safeParseAPIResponse(apiRes);
            if (!Array.isArray(data)) data = [];

            let yeniParfum = null;
            if (data.length > 0) {
                for(let p of data) {
                    if(!currentShownParfums.includes(p.ad) && !rejectedParfums.includes(p.ad)) {
                        yeniParfum = p;
                        break;
                    }
                }
                if(!yeniParfum) yeniParfum = data[0];
            }

            if(yeniParfum) {
                const oldIdx = currentShownParfums.indexOf(oldParfumName);
                if(oldIdx > -1) currentShownParfums[oldIdx] = yeniParfum.ad;
                else currentShownParfums.push(yeniParfum.ad);

                cardContainer.innerHTML = generateCardHTML(yeniParfum, index);
            } else {
                cardContainer.innerHTML = `<div class="p-6 text-center text-red-500 font-bold mt-10">Alternatif bulunamadı.</div>`;
            }

        } catch (e) {
            console.error(e);
            cardContainer.innerHTML = `<div class="p-6 text-center text-red-500 font-bold mt-10">Bağlantı hatası oluştu.</div>`;
        }
    }

    async function submitForm() {
        const name = document.getElementById('input-name').value.trim();
        const age = parseInt(document.getElementById('input-age').value || '0');
        const gender = document.getElementById('input-gender').value;
        const resultsDiv = document.getElementById('results'); 
        
        const payload = { Ad:name, Yas:age, Cinsiyet:gender, Notalar:selectedNotalar, SevdigiParfumler:selectedParfums };
        
        currentShownParfums = [];
        rejectedParfums = [];
        
        showStep('step-loading');
        
        try{
            const apiRes = await fetch(`${API_BASE}/api/parfum/oneri`, { 
                method: 'POST', 
                headers: { 'Content-Type':'application/json' }, 
                body: JSON.stringify(payload) 
            });
            
            let data = await safeParseAPIResponse(apiRes);
            if (!Array.isArray(data)) data = []; 

            await new Promise(r => setTimeout(r, 1200));

            resultsDiv.innerHTML=''; 
            
            if(data.length === 0) {
                 resultsDiv.innerHTML=`
                 <div class="col-span-full p-6 rounded-2xl bg-red-50/50 border border-red-100 text-center">
                    <h5 class="text-red-600 font-bold text-lg mb-2">Öneriler Alınamadı</h5>
                    <p class="text-slate-600">Yapay zeka şu anda format dışı bir cevap döndü veya veri getirilemedi. Lütfen tekrar deneyin.</p>
                 </div>`; 
            } else {
                data.forEach((p,idx)=>{ 
                    currentShownParfums.push(p.ad);
                    const col = document.createElement('div'); 
                    col.className = 'w-full'; 
                    col.innerHTML = generateCardHTML(p, idx); 
                    resultsDiv.appendChild(col); 
                });
            }
            
            showStep(4);
            
        }catch(e){ 
            console.error(e); 
            resultsDiv.innerHTML=`<div class="col-span-full p-6 rounded-2xl bg-red-50/50 border border-red-100 text-center text-red-600 font-bold">Ağ bağlantısı koptu veya sunucuya ulaşılamadı.</div>`; 
            showStep(4);
        }
    }

    showStep(1);
