using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using school.Models;
using school.Classes;
using System.Data.SqlClient;
using System.Configuration;
using System.Linq.Expressions;

namespace school.Controllers
{
    public class smkItmesController : Controller
    {
        private GlobalInfo db = new GlobalInfo();

        string str_connection = ConfigurationManager.ConnectionStrings["GlobalInfo"].ConnectionString;
        //
        // GET: /smkItmes/

        public ActionResult Index()
        {
            if (Session["USERNAME"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(db.smk_itens.ToList());
        }

        //
        // GET: /smkItmes/Details/5
        public ActionResult buscaItem(string cod_smk ,string op) 
        {

            List<materia_primas> materia_primaArr= new List<materia_primas>();
            switch (op) 
            
            {
                case "item_smk":
                    string conteudo1 = "0" ;  
                    materia_primaArr = getMateriaPrimas(cod_smk) ;
                    if (materia_primaArr.Count > 0)
                    {
                        for(var i = 0 ; i < materia_primaArr.Count; i++)
                        {
                            conteudo1 = materia_primaArr[i].descricao + "<input type='hidden' name='materia_primas_id' id='materia_primas_id' value='" + materia_primaArr[i].id + "' />";
                        }

                        return Json(new { conteudo = conteudo1 });
                    }
                    else {

                        return Json(new { conteudo = "0" });
                    }
                   
                    break;
                case "verificaCadastro":
                        return Json(new { conteudo = "0" });        
                    break;
                default:
                        return Json(new { conteudo = "Falta parâmetros" });
                    break;
            
            
            }
            
            if (Session["USERNAME"] == null)
            {
                return Json(new { ok = "failed" });

            }
            else
            {
                return Json(new { ok = "success" });
            }

        
         }

        public ActionResult calculaGasto(string cod, string codSubProduto , string op,string s , string i , string d, string e , string f ,string v) 
        {

            sub_produtos subProdutosObj = new sub_produtos();
            subProdutosObj = db.sub_produtos.Find(Convert.ToInt32(codSubProduto));
            decimal total = 0;
            string opString = op;
            double quantidadeLargura = 0;
            double quantidadeComprimento = 0;
            double quantidadeArea = 0;
            int codInt = Convert.ToInt32(cod);

            if ((opString == "madeira") || (opString == "mdf"))
            {
                total = Convert.ToDecimal(subProdutosObj.area * subProdutosObj.perda / 100 + subProdutosObj.area);
                
            }else if(opString == "fita_borda")
		    {
                List<perda> perdaArr= new List<perda>();


                
                perdaArr = (from x in db.perdas
                            where x.materia_primas_id == codInt
                     select x
                   ).ToList();
                for(var j = 0; j < perdaArr.Count ; j++)
                {
                    if((Convert.ToInt32(s) == 1) && ( Convert.ToInt32(i) == 1)){
                        quantidadeLargura = subProdutosObj.largura_acabada*2 + perdaArr[j].quantidade*2;

                    }
                    else if ((Convert.ToInt32(s) == 1) || (Convert.ToInt32(i) == 1))
                    {
                        
                        quantidadeLargura = subProdutosObj.largura_acabada + perdaArr[j].quantidade ;
                    }
                    if(( Convert.ToInt32(d) == 1) && ( Convert.ToInt32(e) == 1))
                    {
                        quantidadeComprimento = subProdutosObj.comprimento_acabada*2 + perdaArr[j].quantidade*2;
                    }
                    else if ((Convert.ToInt32(d) == 1) || (Convert.ToInt32(e) == 1))
                    {
                        quantidadeComprimento = subProdutosObj.comprimento_acabada  + perdaArr[j].quantidade;
                    }
                 }
                    total = Convert.ToDecimal(((quantidadeComprimento + quantidadeLargura)* subProdutosObj.quantidade)/1000);

            }else if(opString == "polistein")
            {
                   
                    List<consumos> consumosArr = new List<consumos>();
                    
                     consumosArr = (from x in db.consumos
                                    where x.materia_primas_id == codInt
                                    select x
                                    ).ToList();
                    for (var j = 0; j < consumosArr.Count ; j++)
                    {
                        if(( Convert.ToInt32(s) == 1) && ( Convert.ToInt32(i) == 1))
                        {
                            quantidadeLargura = ((((double)(subProdutosObj.largura_acabada) / 1000.0) * ((double)(subProdutosObj.espessura_acabada) / 1000)) * 2) * (consumosArr[j].quantidade * 2);
                        }
                        else if ((Convert.ToInt32(s) == 1) || (Convert.ToInt32(i) == 1))
                        {
                            quantidadeLargura = (((double)(subProdutosObj.largura_acabada)/1000.0)*((double)(subProdutosObj.espessura_acabada)/1000.0))*consumosArr[j].quantidade;
                        }
                        if((Convert.ToInt32(d) == 1) && (Convert.ToInt32(e) == 1))
                        {
                        
                            quantidadeComprimento = (((double)(subProdutosObj.comprimento_acabada)/1000.0)*((double)(subProdutosObj.espessura_acabada)/1000.0)*2)*(consumosArr[j].quantidade*2);
                        }
                        else if ((Convert.ToInt32(d) == 1) || (Convert.ToInt32(e) == 1))
                        {
                            quantidadeComprimento = (((double)(subProdutosObj.comprimento_acabada) / 1000.0) * ((double)(subProdutosObj.espessura_acabada) / 1000.0)) * (consumosArr[j].quantidade * 2);
                        }
                        if((Convert.ToInt32(f) == 1) && (Convert.ToInt32(v) == 1))
                        {
                            quantidadeArea = ((((double)(subProdutosObj.largura_acabada) / 1000.0) * ((double)(subProdutosObj.espessura_acabada) / 1000.0)) * 2) * (consumosArr[j].quantidade * 2);
                        }
                        else if ((Convert.ToInt32(f) == 1) || (Convert.ToInt32(v) == 1))
                        {
                            quantidadeArea = (((double)(subProdutosObj.comprimento_acabada) / 1000.0) * ((double)(subProdutosObj.espessura_acabada) / 1000.0)) * (consumosArr[j].quantidade * 2);
                        }                 
                    }
                    total = Convert.ToDecimal(((quantidadeComprimento + quantidadeLargura + quantidadeArea )*subProdutosObj.quantidade)); 
			       
            }else if(opString == "cola_hotmelt")
            {
                List<sub_produtos_materia_primas> sub_produtos_materia_primasArr = new List<sub_produtos_materia_primas>();

                sub_produtos_materia_primasArr = (from x in db.sub_produtos_materia_primas
                                        where x.sub_produtos_id == subProdutosObj.id &&
                                              x.calculo == "fita_borda"
                                         select x
                                            ).ToList();
                for(var j = 0 ; j < sub_produtos_materia_primasArr.Count;j++){
                    List<consumos> consumosArr = new List<consumos>();

                    consumosArr = (from x in db.consumos
                                   where x.materia_primas_id == codInt
                                   select x).ToList();

                    for( var k = 0 ; k < consumosArr.Count;k++){
                        total = Convert.ToDecimal((subProdutosObj.quantidade * ((double)(subProdutosObj.espessura_acabada) / 1000.0)) * consumosArr[k].quantidade);
                    }

                }
                
                                    
            }else if(opString == "cola_contato"){
                
                List<consumos> consumosArr = new List<consumos>();

                    consumosArr = (from x in db.consumos
                                   where x.materia_primas_id == codInt
                                   select x).ToList();
                for(var j = 0 ; j < consumosArr.Count;j++){
                    
                    total = Convert.ToDecimal((double)(subProdutosObj.area) * consumosArr[j].quantidade);
                    
                }
                
            }
            else if(opString == "laminado")
		    {
				//area * perda cadastrada no subproduto
				//$total = $dadosSubProduto['area'] * ($dadosQuantidadePerda['quantidade']);
                total = Convert.ToDecimal((double)(subProdutosObj.area) * (double)(subProdutosObj.area) / 1000.0 + (double)(subProdutosObj.area));
		    }
			    
		    
            else {

                total = -1;
            }


            return Json(new { result = total });
        }
        [HttpPost]
        public ActionResult registerSubProdutos(string codSubProduto, string idSmkItem, string materia_primas_id, string quantidade, string calculo, string superior, string inferior, string esquerda, string direita)
        {

            string str_query = @"INSERT INTO sub_produtos_materia_primas 
                                   (sub_produtos_id,materia_primas_id,quantidade,calculo ,
                                    superior,inferior,esquerda,direita)
                                  VALUES (@sub_produtos_id,@materia_primas_id,@quantidade,@calculo,
                                          @superior ,@inferior ,@esquerda , @direita

                                    )";

            using (SqlConnection conn = new SqlConnection(str_connection))
            using (SqlCommand cmd = new SqlCommand(str_query, conn))
            {

                cmd.Parameters.AddWithValue("@sub_produtos_id", Convert.ToInt32(codSubProduto));
                cmd.Parameters.AddWithValue("@materia_primas_id", Convert.ToInt32(materia_primas_id));
                cmd.Parameters.AddWithValue("@quantidade", Convert.ToDouble(quantidade));
                cmd.Parameters.AddWithValue("@calculo",calculo );
                cmd.Parameters.AddWithValue("@superior", Convert.ToInt32(superior));
                cmd.Parameters.AddWithValue("@inferior", Convert.ToInt32(inferior));
                cmd.Parameters.AddWithValue("@esquerda", Convert.ToInt32(esquerda));
                cmd.Parameters.AddWithValue("@direita", Convert.ToInt32(direita));
               
                 conn.Open();


                cmd.ExecuteNonQuery();
                conn.Close();
                conn.Dispose();


                conn.Close();
             }

            return  RedirectToAction("Details", new { id = Convert.ToInt32(idSmkItem) });                                                              
             
        
        }

        public List<materia_primas> getMateriaPrimas(string cod_smk) 
        {   
            List<materia_primas> materia_primaArr= new List<materia_primas>();
           string str_query = @"select * from materia_primas where codigo_smk = @cod_smk";

            using (SqlConnection conn = new SqlConnection(str_connection))
            using (SqlCommand cmd = new SqlCommand(str_query, conn))
            {
                cmd.Parameters.AddWithValue("@cod_smk", cod_smk);

                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    materia_primas materia_primasObj = new materia_primas();
                    int materiaPrimaId = Convert.ToInt32(reader["id"]);
                    materia_primasObj.id = materiaPrimaId;
                    materia_primasObj.descricao = reader["descricao"].ToString(); 
                    materia_primaArr.Add(materia_primasObj);

                }

            }

            return materia_primaArr;
        }

        public ViewResult Details(int id)
        {

            smkProductView smkProductView = new smkProductView();
            List<smkSubProdus> smkSubProdus = new List<smkSubProdus>();


           
           

            List<smkAccessory> smkSubAccessory = new List<smkAccessory>();
            List<smkAccessory> smkAccessory1 = new List<smkAccessory>();
            List<smkAccessory> smkAccessory2 = new List<smkAccessory>();


            smkProductView.id = id;
            string str_query = @"SELECT sp.id, sp.descricao, comprimento_acabada, largura_acabada, espessura_acabada, comprimento_bruto, 
								largura_bruto, espessura_bruto, quantidade, AREA, perda, 
									smk_itens_id, csp.descricao AS csp_descricao FROM sub_produtos sp 
									JOIN categoria_sub_produtos csp ON csp.id = sp.categoria_sub_produtos_id
									WHERE sp.smk_itens_id =@id ORDER BY csp.descricao";

            using (SqlConnection conn = new SqlConnection(str_connection))
            using (SqlCommand cmd = new SqlCommand(str_query, conn))
            {
                cmd.Parameters.AddWithValue("@id", id);

                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int nId = Convert.ToInt32(reader["id"]);
                    string descricao = reader["descricao"].ToString();
                    string comprimento_acabada = reader["comprimento_acabada"].ToString();
                    string largura_acabada = reader["largura_acabada"].ToString();
                    string espessura_acabada = reader["espessura_acabada"].ToString();
                    string comprimento_bruto = reader["comprimento_bruto"].ToString();
                    string largura_bruto = reader["largura_bruto"].ToString();
                    string espessura_bruto = reader["espessura_bruto"].ToString();
                    double quantidade = Convert.ToDouble(reader["quantidade"]);
                    string area = reader["Area"].ToString();
                    string perda = reader["perda"].ToString();
                    int smkItemId = Convert.ToInt32(reader["smk_itens_id"]);
                    string csp_descricao = reader["csp_descricao"].ToString();

                    smkSubProdus objsubProdus = new smkSubProdus();


                    objsubProdus.id = nId;
                    objsubProdus.sp_descricao = descricao;
                    objsubProdus.comprimento_acabada = comprimento_acabada;
                    objsubProdus.espessura_acabada = espessura_acabada;
                    objsubProdus.comprimento_bruto = comprimento_bruto;
                    objsubProdus.largura_bruto = largura_bruto;
                    objsubProdus.quantidade = quantidade;
                    objsubProdus.area = area;
                    objsubProdus.perda = perda;
                    objsubProdus.smk_itens_id = smkItemId;
                    objsubProdus.csp_descricao = csp_descricao;
                    objsubProdus.largura_acabada = largura_acabada;

                    smkSubAccessory = getSmkProduct(nId);
                    objsubProdus.smkAccessory = smkSubAccessory;
                    smkSubProdus.Add(objsubProdus);




                }
                smkProductView objsmkProductView = new smkProductView();

                smkAccessory objsmkAccessory = new smkAccessory();

                smkAccessory1 = getSmkAccessory1(id);
                smkAccessory2 = getSmkAccessory2(id);

                smkProductView.smkSubProdus = smkSubProdus;
                smkProductView.smkAccessory1 = smkAccessory1;
                smkProductView.smkAccessory2 = smkAccessory2;


                conn.Close();
            }
            smk_itens smk_itens = db.smk_itens.Find(id);
            smkProductView.smk_itensObj = smk_itens;

            return View(smkProductView);
        }
        public ActionResult deletaSubProdutos( string id1 ,string id2 ,string acao ,string idSmkItem)
        {
             switch (acao){
                    case "inserir":
                            
                         
                        break;

                    case "editar":
                            break;

                    case "apagar":
                            string str_query = @"delete from sub_produtos_materia_primas where sub_produtos_id = @sub_produtos_id
                                                and materia_primas_id = @materia_primas_id" ;               
                               
                            using (SqlConnection conn = new SqlConnection(str_connection))
                            using (SqlCommand cmd = new SqlCommand(str_query, conn))
                            {

                                cmd.Parameters.AddWithValue("@sub_produtos_id", Convert.ToInt32(id1));
                                cmd.Parameters.AddWithValue("@materia_primas_id", Convert.ToInt32(id2));

                                conn.Open();


                                cmd.ExecuteNonQuery();
                                conn.Close();
                                conn.Dispose();


                                conn.Close();
                            }
                         
                         break;

                    case "default":
                        
                        break;
            }
            return Json(new { ok = "success" });

        }
       
        public ActionResult deletaAcessorios(int id , string acao , int idSmkItem) {

            switch (acao)
            {
                case "inserir":


                    break;

                case "editar":
                    break;

                case "apagar":
                    string str_query = @"delete from acessorios where id = @id";

                    using (SqlConnection conn = new SqlConnection(str_connection))
                    using (SqlCommand cmd = new SqlCommand(str_query, conn))
                    {


                        cmd.Parameters.AddWithValue("@id", id);

                        conn.Open();


                        cmd.ExecuteNonQuery();
                        conn.Close();
                        conn.Dispose();


                        conn.Close();
                    }

                    break;

                case "default":

                    break;
            }
            return Json(new { ok = "success" });
        }
        
        public ActionResult addSubProduct(string codSmk) {

            addSubProductView addSubProductViewObj = new addSubProductView();
            int codsmkInt = Convert.ToInt32(codSmk);

            smk_itens smk_itensObj = new smk_itens();


            smk_itensObj = (from x in db.smk_itens
                            where x.id ==  codsmkInt
                            select x
                            ).FirstOrDefault();

            List<categoria_sub_produtos> categoria_sub_produtosArr = new List<categoria_sub_produtos>();
            categoria_sub_produtosArr = (from x in db.categoria_sub_produtos
                                         select x).ToList();

            addSubProductViewObj.categoria_sub_produtosArr = categoria_sub_produtosArr;
            addSubProductViewObj.smk_itensObj = smk_itensObj;

            return View(addSubProductViewObj);
        }

        public ActionResult registerAccessory(string acao , string idSmkItem , string calculo , string codigo_smk , string materia_primas_id, string quantidade) {
             
              switch (acao){
                    case "inserir":
                            
                         acessorios acessoriosObj = new acessorios
                            {
                                smk_itens_id =Convert.ToInt32(idSmkItem),
                                materia_primas_id = Convert.ToInt32(materia_primas_id),
                                quantidade = Convert.ToDouble(quantidade)
                            };

                            
                            db.acessorios.Add(acessoriosObj);


                            // Submit the change to the database. 
                            try
                            {
                                db.SaveChanges();
                            }
                            catch (Exception e)
                            {
                              
                               db.SaveChanges();
                            }
                  
                        break;
                 
                }


              return RedirectToAction("Details", "smkItmes", new { id = idSmkItem });
        }

        public ActionResult AdicionarSub(string acao , string id , string descricao , string tipo_calculo , string comprimento_acabada ,string largura_acabada , string espessura_acabada , string comprimento_bruto , string largura_bruto , string  espessura_bruto , string quantidade , string area , string perda , string smk_itens_id , string categoria_sub_produtos_id ) {
            int codSubProduto = Convert.ToInt32(id);  
            switch (acao)
            {
                case "inserir":

                    sub_produtos sub_produtosObj = new sub_produtos
                    {
                        descricao = descricao,
                        comprimento_acabada = Convert.ToInt32(comprimento_acabada),
                        largura_acabada = Convert.ToInt32(largura_acabada),
                        espessura_acabada = Convert.ToInt32(espessura_acabada),
                        comprimento_bruto = Convert.ToInt32(comprimento_bruto),
                        largura_bruto = Convert.ToInt32(largura_bruto),
                        espessura_bruto = Convert.ToInt32(espessura_bruto),
                        quantidade = Convert.ToInt32(espessura_acabada),
                        area = Convert.ToDouble(area),
                        perda = Convert.ToInt32(perda),
                        smk_itens_id = Convert.ToInt32(smk_itens_id),
                        categoria_sub_produtos_id = Convert.ToInt32(categoria_sub_produtos_id),
                    };
                    break;
                   case "editar":
                       {
                            decimal total = 0;
                            string op = "";
                            int quantidadeLargura = 0;
                            int quantidadeComprimento = 0;
                            int quantidadeArea = 0;
                            decimal totalFitaBorda;
                            sub_produtos subProdutosObj = new sub_produtos();
                            int subProdutosId = Convert.ToInt32(id);

                            var query =
                                from ord in db.sub_produtos
                                where ord.id == subProdutosId
                                select ord;

                            // Execute the query, and change the column values 
                            // you want to change. 
                            foreach (sub_produtos ord in query)
                            {
                                ord.descricao = descricao;
                                ord.largura_acabada = Convert.ToInt32(largura_acabada);
                                ord.espessura_acabada = Convert.ToInt32(espessura_acabada);
                                ord.comprimento_bruto = Convert.ToInt32(comprimento_bruto);
                                ord.largura_bruto = Convert.ToInt32(largura_bruto);
                                ord.espessura_bruto = Convert.ToInt32(espessura_bruto);
                                ord.quantidade = Convert.ToInt32(espessura_acabada);
                                ord.area = Convert.ToDouble(area);
                                ord.perda = Convert.ToInt32(perda);
                                ord.smk_itens_id = Convert.ToInt32(smk_itens_id);
                                ord.categoria_sub_produtos_id = Convert.ToInt32(categoria_sub_produtos_id);
                                // Insert any additional changes to column values.
                            }

                        // Submit the changes to the database. 
                            try
                            {
                                db.SaveChanges();
                            }
                            catch (Exception e)
                            {
                           
                            }
   
				            sub_produtos_materia_primas sub_produtos_materia_primasObj = new sub_produtos_materia_primas();
                            sub_produtos_materia_primasObj = (from x in db.sub_produtos_materia_primas
                                                              where x.sub_produtos_id == codSubProduto
                                                              select x).FirstOrDefault();

                            op = sub_produtos_materia_primasObj.calculo;
                      

                            if ((op == "madeira") || (op == "mdf"))
                            {
                                total = Convert.ToDecimal(Convert.ToDecimal(area) * Convert.ToDecimal(perda) / 100 + Convert.ToDecimal(area));
                
                            }else if(op == "fita_borda")
                            {
                                perda perdaObj = new perda();

                                int s = Convert.ToInt32(sub_produtos_materia_primasObj.superior);
                                int i = Convert.ToInt32(sub_produtos_materia_primasObj.inferior);
                                int e = Convert.ToInt32(sub_produtos_materia_primasObj.esquerda);
                                int d = Convert.ToInt32(sub_produtos_materia_primasObj.direita);

                                int materia_primasId = Convert.ToInt32(sub_produtos_materia_primasObj.materia_primas_id);

                                perdaObj = (from x in db.perdas
                                            where x.materia_primas_id == materia_primasId
                                     select x
                                   ).FirstOrDefault();

                                int dadosQuantidadePerda = Convert.ToInt32(perdaObj.quantidade);
                            
                                    if(( s == 1) && ( i == 1))
								    {
									    quantidadeLargura = Convert.ToInt32(largura_acabada)*2 + dadosQuantidadePerda*2;	
								    }else if(( s == 1) || ( i == 1))
								    {
									    quantidadeLargura = Convert.ToInt32(largura_acabada)+ dadosQuantidadePerda;
								    }
								
								    if(( d == 1) && ( e == 1))
								    {
									    quantidadeComprimento = Convert.ToInt32(comprimento_acabada) *2 + dadosQuantidadePerda * 2;	
								    }else if(( d == 1) || ( e == 1))
								    {
									    quantidadeComprimento = Convert.ToInt32(comprimento_acabada) + dadosQuantidadePerda;	
							        }


                                total = ((quantidadeComprimento + quantidadeLargura)* Convert.ToInt32(quantidade))/1000;//converto milimetros para metros
							    totalFitaBorda = total;
                            }else if( op == "cola_hotmelt")
						    {	
							    //verifico se havia cola hotmelt cadastrada, pois haverá mudança por causa da quantidade de fita alterada também
							
                                 sub_produtos_materia_primasObj = (from x in db.sub_produtos_materia_primas
                                                              where x.sub_produtos_id == codSubProduto && 
                                                                    x.calculo == "cola_hotmelt"
                                                              select x).FirstOrDefault();
                                int materia_primas_id = sub_produtos_materia_primasObj.materia_primas_id; 
                            


                                consumos consumosObj = new consumos();
                                consumosObj = (from x in db.consumos
                                               where x.materia_primas_id == materia_primas_id select x).FirstOrDefault();

                                decimal consumousQuantidade = Convert.ToDecimal(consumosObj.quantidade);
                                total = (Convert.ToDecimal(espessura_acabada)/1000) * consumousQuantidade;
                            }
                            else if(op == "laminado"){
                                total = (Convert.ToDecimal(area) * Convert.ToDecimal(perda)/100) + Convert.ToDecimal(area);
                            
                            }else if( op == "polistein"){
                                decimal quantidadeComprimentoDecimal =0;
							    decimal quantidadeLarguraDecimal=0;
							    decimal quantidadeAreaDecimal=0;

						        int s = Convert.ToInt32(sub_produtos_materia_primasObj.superior);
                                int i = Convert.ToInt32(sub_produtos_materia_primasObj.inferior);
                                int d = Convert.ToInt32(sub_produtos_materia_primasObj.direita);
                                int e = Convert.ToInt32(sub_produtos_materia_primasObj.esquerda);
                                int f = Convert.ToInt32(sub_produtos_materia_primasObj.frente);
                                int v = Convert.ToInt32(sub_produtos_materia_primasObj.verso);
                           
                                sub_produtos_materia_primasObj = (from x in db.sub_produtos_materia_primas
                                                                  where x.sub_produtos_id == codSubProduto && x.calculo == "polistein"
                                                                  select x).FirstOrDefault();
                                int codMateriaPrimaPolistein =  Convert.ToInt32(sub_produtos_materia_primasObj.materia_primas_id);
                                consumos  consumosObj = new consumos();
                                consumosObj = (from x in db.consumos 
                                               where x.materia_primas_id == codMateriaPrimaPolistein
                                               select x).FirstOrDefault();
                                decimal polisteinQuantitadeDecimal = Convert.ToDecimal(consumosObj.quantidade) ;
                                if(( s == 1) && ( i == 1)){
                                   quantidadeLarguraDecimal = (Convert.ToDecimal(largura_acabada)/1000 )* (Convert.ToDecimal(espessura_acabada)/1000*2)*polisteinQuantitadeDecimal*2 ;	
						
                                }else if(( s == 1) || ( i == 1))
						        {
							        quantidadeLarguraDecimal= ((Convert.ToDecimal(largura_acabada)/1000 ) * (Convert.ToDecimal(espessura_acabada)/1000))* polisteinQuantitadeDecimal;
						        }
                                if(( d == 1) && ( e == 1)){
                                    quantidadeComprimentoDecimal = (((Convert.ToDecimal(comprimento_acabada)/1000) * (Convert.ToDecimal(espessura_acabada)/1000))*2) * polisteinQuantitadeDecimal*2;	
						
                                }else if(( d == 1) || ( e == 1))
						        {
									    quantidadeComprimentoDecimal = ((Convert.ToDecimal(comprimento_acabada)/1000) * (Convert.ToDecimal(espessura_acabada)/1000))* polisteinQuantitadeDecimal;
									    //echo $quantidadeComprimento;	
						        }
							    if(( f == 1) && ( v == 1))
							    {
									    quantidadeAreaDecimal = (((Convert.ToDecimal(comprimento_acabada)/1000) * (Convert.ToDecimal(largura_acabada)/1000))*2) * (polisteinQuantitadeDecimal * 2);	
							    }else if(( f == 1) || ( v == 1))
							    {
									    quantidadeAreaDecimal = ((Convert.ToDecimal(comprimento_acabada)/1000) * (Convert.ToDecimal(largura_acabada)/1000)) * (polisteinQuantitadeDecimal);
							    }	
							
							    total = ((quantidadeComprimentoDecimal + quantidadeLarguraDecimal + quantidadeAreaDecimal)*Convert.ToDecimal(quantidade));
                            }else if( op == "cola_contato"){

                                consumos consumosObj = new consumos();
                                int materiia_primas_id =  Convert.ToInt32(sub_produtos_materia_primasObj.materia_primas_id);
                                consumosObj = (from x in db.consumos
                                               where x.materia_primas_id == materiia_primas_id
                                               select x).FirstOrDefault();
                                total = Convert.ToDecimal(area)* Convert.ToDecimal(consumosObj.quantidade);
                            }else{
                            }
                            int materiaPrimasId = Convert.ToInt32(sub_produtos_materia_primasObj.materia_primas_id);
                            int subprodutosId = sub_produtos_materia_primasObj.sub_produtos_id;
                            var query1 =
                                from ord in db.sub_produtos_materia_primas
                                where ord.materia_primas_id == materiaPrimasId && ord.sub_produtos_id == subprodutosId
                                select ord;


                            // Execute the query, and change the column values 
                            // you want to change. 
                            double totalMateria = Convert.ToDouble(total);
                            foreach (sub_produtos_materia_primas ord in query1)
                            {
                                ord.quantidade = totalMateria;
                       
                            }

                            // Submit the changes to the database. 
                            try
                            {
                                db.SaveChanges();
                            }
                            catch (Exception e)
                            {
                           
                            }
                           break;
                       }
                case "apagar":
                    {
                        int idInt = Convert.ToInt32(id);
                        var deleteOrderDetails =
                            from details in db.sub_produtos
                            where details.id == idInt
                            select details;

                        foreach (var detail in deleteOrderDetails)
                        {
                            db.sub_produtos.Remove(detail);
                        }

                        try
                        {
                            db.SaveChanges();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                            // Provide for exceptions.
                        }
                    
                    }
                    break;
                case "default":
                    {
                        
                        break;
                    }
                  

            }
            
            return RedirectToAction("Details", "smkItmes", new { id = smk_itens_id });
        }

        public ActionResult registerAcessorio(string codSmk){

            smk_itens smk_itensObj = new smk_itens();
            int codsmkInt = Convert.ToInt32(codSmk);

            smk_itensObj = (from x in db.smk_itens
                            where x.id == codsmkInt
                            select x
                           ).FirstOrDefault();


            return View(smk_itensObj);
        }

        public List<smkAccessory> getSmkProduct(int id)
        {
            List<smkAccessory> smkAccessory = new List<smkAccessory>();
            
            string str_query = @"SELECT *
												FROM sub_produtos_materia_primas spmp
												JOIN materia_primas mp ON mp.id = spmp.materia_primas_id
												WHERE sub_produtos_id = @id";

            using (SqlConnection conn = new SqlConnection(str_connection))
            using (SqlCommand cmd = new SqlCommand(str_query, conn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int nId = Convert.ToInt32(reader["id"]);
                    string descricao = reader["codigo_smk"].ToString() + "-" + reader["descricao"].ToString();
                    string unidade = reader["unidade"].ToString();
                    int sub_produtos_id = Convert.ToInt32(reader["sub_produtos_id"]);
                    int materia_primas_id = Convert.ToInt32(reader["materia_primas_id"]);


                    smkAccessory objsmkAccessory = new smkAccessory();


                    objsmkAccessory.id = nId;
                    objsmkAccessory.codigo_smk = descricao;
                    objsmkAccessory.unidade = unidade;
                    objsmkAccessory.sub_produtos_id = sub_produtos_id;
                    objsmkAccessory.materia_primas_id = materia_primas_id;
                    smkAccessory.Add(objsmkAccessory);

                }

                conn.Close();
            }


            return smkAccessory;
        }
        public ActionResult editSubProdus(int id, int modelId)
        {


            sub_produtos sub_produtosObj = new sub_produtos();
            
            sub_produtosObj =  (from x in db.sub_produtos 
                             join p in db.categoria_sub_produtos on x.categoria_sub_produtos_id equals p.id
                             select x).FirstOrDefault();

            addSubProductView addSubProductViewObj = new addSubProductView();


            int codsmkInt = Convert.ToInt32(modelId);

            smk_itens smk_itensObj = new smk_itens();


            smk_itensObj = (from x in db.smk_itens
                            where x.id == codsmkInt
                            select x
                            ).FirstOrDefault();

            List<categoria_sub_produtos> categoria_sub_produtosArr = new List<categoria_sub_produtos>();
            categoria_sub_produtosArr = (from x in db.categoria_sub_produtos
                                         select x).ToList();

            addSubProductViewObj.categoria_sub_produtosArr = categoria_sub_produtosArr;
            addSubProductViewObj.smk_itensObj = smk_itensObj;
            addSubProductViewObj.sub_produtosObj = sub_produtosObj;
            addSubProductViewObj.id = id;
            addSubProductViewObj.codigoSmk = modelId;

            return View(addSubProductViewObj);
        }
        public List<smkAccessory> getSmkAccessory1(int id)
        {
            List<smkAccessory> smkAccessory = new List<smkAccessory>();
           
            string str_query = @"SELECT *
														FROM acessorios ac
														JOIN smk_itens si ON si.id = ac.smk_itens_id
														JOIN materia_primas mp ON mp.id = ac.materia_primas_id 
														JOIN categoria_materia_primas cmp on cmp.id = mp.categoria_materia_primas_id
														where ac.smk_itens_id = @id order by cmp.descricao, mp.descricao";

            using (SqlConnection conn = new SqlConnection(str_connection))
            using (SqlCommand cmd = new SqlCommand(str_query, conn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int nId = Convert.ToInt32(reader["id"]);
                    string descricao = reader["codigo_smk"].ToString() + "-" + reader.GetString(10);
                    string unidade = reader["unidade"].ToString();

                    smkAccessory objsmkAccessory = new smkAccessory();
                    objsmkAccessory.id = nId;
                    objsmkAccessory.codigo_smk = descricao;
                    objsmkAccessory.unidade = unidade;
                    objsmkAccessory.quantitide = Convert.ToDecimal(reader["quantidade"]);
                    objsmkAccessory.categoria = reader.GetString(14);
                    smkAccessory.Add(objsmkAccessory);

                }

                conn.Close();
            }


            return smkAccessory;
        }
        // GET: /smkItmes/Create
        public List<smkAccessory> getSmkAccessory2(int id)
        {
            List<smkAccessory> smkAccessory = new List<smkAccessory>();

            string str_query = @"SELECT si.codigo_smk, mp.descricao, cmp.descricao AS categoria, mp.unidade as unidade, t1.sumQuantidade AS quantidade, mp.codigo_smk AS mp_codigo_smk
                                          FROM sub_produtos_materia_primas spmp
                                          JOIN sub_produtos sp ON sp.id = spmp.sub_produtos_id
                                          JOIN materia_primas mp ON mp.id = spmp.materia_primas_id
                                          JOIN categoria_materia_primas cmp ON cmp.id = mp.categoria_materia_primas_id
                                          JOIN smk_itens si ON si.id = sp.smk_itens_id
                                          JOIN (
                                         SELECT SUM( spmp.quantidade ) AS sumQuantidade, mp.codigo_smk AS codigo_smk
                                           FROM sub_produtos_materia_primas spmp
                                           JOIN materia_primas mp ON mp.id = spmp.materia_primas_id
                                          GROUP BY mp.codigo_smk ) t1 ON t1.codigo_smk = mp.codigo_smk
                                         WHERE si.id = @id
                                         ORDER BY cmp.descricao,mp.descricao";

            using (SqlConnection conn = new SqlConnection(str_connection))
            using (SqlCommand cmd = new SqlCommand(str_query, conn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {

                    string descricao = reader["codigo_smk"].ToString() + "-" + reader["descricao"].ToString();
                    string unidade = reader["unidade"].ToString();
                    decimal quantidade = Convert.ToDecimal(reader["quantidade"]);
                    smkAccessory objsmkAccessory = new smkAccessory();

                    objsmkAccessory.codigo_smk = descricao;
                    objsmkAccessory.quantitide = quantidade;
                    objsmkAccessory.unidade = unidade;

                    smkAccessory.Add(objsmkAccessory);

                    smkAccessory.Add(objsmkAccessory);

                }

                conn.Close();


                return smkAccessory;
            }
        }
        public ActionResult Create()
        {
            return View();
        }

        public ActionResult addSubProdus(int id, int modelId) 
        {

            
            smk_itens smk_itensObj = new smk_itens();

            smk_itensObj = db.smk_itens.Find(modelId);

            smkaddSubProdutos smkaddSubProdutosObj = new smkaddSubProdutos();
            smkaddSubProdutosObj.smkItemObj = smk_itensObj;
            smkaddSubProdutosObj.cod = id;


            return View(smkaddSubProdutosObj);
        }

        public ActionResult editAccessory(int id, int idSmkItem) {
            acessoriosEdit acessoriosEditObj = new acessoriosEdit();
            
            acessorios acessoriosObj = new  acessorios();
            acessoriosEditObj = (from x in db.acessorios
                             join ma in db.materia_primas on x.materia_primas_id equals ma.id
                             where x.id == id
                             select new acessoriosEdit
                             {
                                    
                                    id = x.id,
                                    smk_itens_id = x.smk_itens_id ,
                                    materia_primas_id = x.materia_primas_id ,
                                    quantidade = x.quantidade ,
                                    descricao = ma.descricao ,
                                    idSmkItem = idSmkItem
                                        
                             } ).FirstOrDefault();

            return View(acessoriosEditObj);
        }
        // POST: /smkItmes/Create

        [HttpPost]
        public ActionResult Create(smk_itens smk_itens)
        {
            if (ModelState.IsValid)
            {
                db.smk_itens.Add(smk_itens);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(smk_itens);
        }
        [HttpPost]
        public ActionResult acessoriosController(string acao , int id, int idSmkItem , double quantidade)
        {
            
             switch (acao){
            

                case "editar":
                        
                       var query =
                                from ord in db.acessorios
                                where ord.id == id
                                select ord;

                            // Execute the query, and change the column values 
                            // you want to change. 
                            foreach (acessorios ord in query)
                            {
                                ord.quantidade = quantidade;
                            }

                        // Submit the changes to the database. 
                            try
                            {
                                db.SaveChanges();
                            }
                            catch (Exception e)
                            {
                           
                            }
   
                    break;
                

                case "default":
                    
                    break;
            }
             return RedirectToAction("Details", new { id = idSmkItem });
        }
        //
        // GET: /smkItmes/Edit/5

        public ActionResult Edit(int id)
        {
            smk_itens smk_itens = db.smk_itens.Find(id);
            return View(smk_itens);
        }

        //
        // POST: /smkItmes/Edit/5

        [HttpPost]
        public ActionResult Edit(smk_itens smk_itens)
        {
            if (ModelState.IsValid)
            {
                db.Entry(smk_itens).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(smk_itens);
        }

        //
        // GET: /smkItmes/Delete/5

        public ActionResult Delete(int id)
        {
            smk_itens smk_itens = db.smk_itens.Find(id);
            return View(smk_itens);
        }

        //
        // POST: /smkItmes/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            smk_itens smk_itens = db.smk_itens.Find(id);
            db.smk_itens.Remove(smk_itens);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}